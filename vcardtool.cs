
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class vcard {
  public string name { get; set; }
  public string version { get; set; }
  private SortedList<string, string> content;

  public vcard(string name, string version, SortedList<string, string> content) {
    this.version = version;  
    this.name = name;
    this.content = content;
  }
  public void dump(StreamWriter sw) {
    sw.WriteLine("BEGIN:VCARD");
    sw.WriteLine("VERSION:" + version);
    foreach(string s in content.Values)
      sw.WriteLine(s);
    sw.WriteLine("END:VCARD");
  }  
}

class vcardtools {

  private static Dictionary<string, vcard> vcards;

  public static void Main() {

    string s, sName, sVersion;
    SortedList<string, string> content;
    vcard vc;
    
    string sInFile = @"./in.vcard";
    string sOutFile =@"./out.vcard";

    vcards = new Dictionary<string, vcard>();

//    string[] asLines = File.ReadAllLines(sInFile);
    string[] vcard = File.ReadAllLines(sInFile);

    StreamWriter sw = new StreamWriter(sOutFile, false, new UTF8Encoding());

    // dump
    Console.WriteLine("# content dump for: {0}" + sInFile);

    StreamReader sr = new StreamReader(sInFile);
//    foreach (string s in asLines) {

    vc = null;
    sName = "";
    sVersion = "";
    content = new SortedList<string, string>();

    while(sr.Peek() >= 0) {
      s = sr.ReadLine();

      if (s == "END:VCARD") {
        Console.WriteLine("[info] adding vcard with key: '{0}'" + sName);
        vc = new vcard(sName, sVersion, content);
      } else if (s.Substring(0, 7) == "VERSION") {
        sVersion = s.Substring(8);
      } else if (s.Substring(0, 2) == "N:") {
        sName = s.Substring(2);
        content.Add(s, s);
      } else if (s == "BEGIN:VCARD") {
        sName = "";
        sVersion = "";
        content.Clear();
      }
    }
    sw.Flush();
  }

  delegate string dName(vcard vc);
  public int dumpSorted(StreamWriter sw) {
    // sort the vcards
    // .Net >= 3.5
    // lambda expression to return function pointer
    int lRet = 0;

    dName d = vc => vc.name;
//    IEnumerable<vcard> sorted = vcards.OrderBy<vcard, string>(d);
//    foreach (vcard v in sorted)
//      v.dump(sw);
    return lRet;
  }

}
