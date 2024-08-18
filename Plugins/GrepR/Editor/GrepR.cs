using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Twonek {
public class GrepR : EditorWindow
{

	[MenuItem("Tools/GrepR by 2nek")]
    static void Init()
    {
        GrepR window = (GrepR)EditorWindow.GetWindow(typeof(GrepR));
        window.Show();
    }

    private int result = 0;
    int cnt = 0;
    const string NAME = "SS";
	string text1 = ""; // String Pattern
	string text2 = "Assets"; // Path
	string text3 = "*.cs";  // File Pattern
	string text4 = "Assets"; // File Pattern for Utility
    private Vector2 _scrollPosition = Vector2.zero;
    string log = "";
    bool regsw;
    bool ignoreCase = true;
	bool  cancel = false;
	bool verbose = false;
	
    public static string ReplaceLastOccurrence(string source, string find, string replace)
    {
        int place = source.LastIndexOf(find);

        if (place == -1)
            return source;

        return source.Remove(place, find.Length).Insert(place, replace);
    }

    void OnGUI()
    {
	    if (UnityEditor.EditorPrefs.HasKey("GrepRStrPat")) {
	    	text1 = UnityEditor.EditorPrefs.GetString("GrepRStrPat");
	    }
	    if (UnityEditor.EditorPrefs.HasKey("GrepRPath")) {
	    	text4 = UnityEditor.EditorPrefs.GetString("GrepRPath");
	    }
	    if (UnityEditor.EditorPrefs.HasKey("GrepRFilePat")) {
	    	text3 = UnityEditor.EditorPrefs.GetString("GrepRFilePat");
	    }
	    var oldtext1 = text1;
	    var oldtext2 = text2;
	    var oldtext3 = text3;

        GUI.SetNextControlName(NAME);
	    text1 = EditorGUILayout.TextField("Search Pattern", text1);

	    GUILayout.BeginHorizontal("box");
        text2 = EditorGUILayout.TextField("Path", text2);
	    if (GUILayout.Button("...", GUILayout.Width(40)))
            text4 = EditorUtility.OpenFolderPanel("Path", text4, "");
	    GUILayout.EndHorizontal();

        string p1 = Application.dataPath;

        p1 = ReplaceLastOccurrence(p1, "/Assets", "");

        p1 = p1 + "/";

        text4 = text4.Replace(p1, "");


        string path = text4;

        if (path == null) path = ".";

        //	Debug.Log("path="+path);
        bool isDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        if (isDirectory == false) return;
        text2 = path;

        text3 = EditorGUILayout.TextField("File Pattern", text3);

        if (cnt == 0)
        {

            EditorGUI.FocusTextInControl(NAME);
        }
        cnt++;

	    GUILayout.BeginHorizontal("box");
        regsw = GUILayout.Toggle(regsw, "Regexp");
	    ignoreCase = GUILayout.Toggle(ignoreCase, "IgnoreCase");
	    verbose = GUILayout.Toggle(verbose, "Verbose");
	    GUILayout.EndHorizontal();
	    GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Find"))
        {

            if (text1 != null && text1 != "")
            {
	            log = ""; Find(path, text3, text1);  // text3 = File Pattern, text1 = String Pattern
	            if (foundCount == 0) {
	            	log=text1 + " not found in " + text3;
	            }
            }
            else
            {
	            log = ("Error: No Search String Specified");
            }
        }
	    if (GUILayout.Button("Cancel"))
	    {
		     cancel = true;
	    }
	    if (GUILayout.Button("Clear")) {
		    log = "";
		    cancel = false;   
	    }
	    GUILayout.EndHorizontal();

        var style = new GUIStyle(EditorStyles.textArea)
        {
            wordWrap = true,
            richText = true,

        };

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.MaxWidth(1000));
        log = EditorGUILayout.TextArea(log, style, GUILayout.ExpandHeight(true));
	    EditorGUILayout.EndScrollView();
        
	    if (oldtext1 != text1)
		    UnityEditor.EditorPrefs.SetString("GrepRStrPat", text1);
	    if (oldtext2 != text2)
		    UnityEditor.EditorPrefs.SetString("GrepRPath", text4);
	    if (oldtext3 != text3)
	        UnityEditor.EditorPrefs.SetString("GrepRFilePat", text3);
    }

	bool running = false;
	int foundCount=0;
	void Find(string path, string filepat, string pat) {
		if (running == true) { 
			log+="Error: Still Running";
		   return;
		}
		running = true;
		foundCount=0;
		Find_(path, filepat, pat, ref foundCount);
		running = false;
	}
	void Find_(string path, string filepat, string pat, ref int count)
    {
        string scriptFolder = path;
        string[] scriptFiles = Directory.GetFiles(scriptFolder, filepat, SearchOption.AllDirectories);
        int scriptCount = scriptFiles.Length;
        int totalLines = 0;
	    cancel = false;
	  
	   
	    if (regsw) {
	    	RegexOptions opt = RegexOptions.None;
		    if (ignoreCase) opt = RegexOptions.IgnoreCase;
		    a = new Regex(pat, opt);
	    }
	    
        foreach (var scriptFile in scriptFiles)
        {
        	if (verbose)
	        	Debug.Log("Finding " + pat + " in " + scriptFile);
        	
        	int foundcount = 0;
	        List<string> list = FindPatInFile(scriptFile, pat, a);
           
	        if (verbose) 
		        Debug.Log(list.Count + " " + pat + " found in " + scriptFile);

	        count +=   list.Count;
            for (int i = 0; i < list.Count; i++)
            {

                log += list[i];
                log += "\n";
            }
	        list.Clear();
	        
            
	        if ( cancel) {
	        	 cancel = false;
	        	break;
	        }

        }
	  
    }

    Regex a;
	bool Match(string line, string pat, Regex a)
    {
        if (regsw)
        {
        	
	        var x = a.Match(line);
            return x.Success;
        }
	    else if (ignoreCase)
        {
            var l = line.ToLower();
            var p = pat.ToLower();
            return l.Contains(p);
        }
        return line.Contains(pat);
    }

	List<string> FindPatInFile(string filePath, string pat, Regex a)
	{
    	
		
		 int found=0;
 
		Task<List<string>> task = Task.Run(()=> {     
			List<string> ls = new List<string>();
			int lineCount = 0;
		
       
        using (StreamReader reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                lineCount++;
	            if (Match(line, pat, a))
                {
		            ls.Add(string.Format("<a href=\"{0}\" line=\"{1}\">{0}:{1}</a>:{2}", filePath, lineCount, line));
		            found++;
                }
	            if (cancel) break;


            }
        }
       
			return ls;
		});
		
		
		 
		return task.Result;
    }

}
}