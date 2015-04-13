/*
The MIT License

Copyright (c) 2015 Martin Pichlmair

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

public static class TextFileReader {
	public static List<string> LoadTextList(string filename) {
		StringReader reader = null; 
		List<string> entries = new List<string>();
		
		TextAsset puzdata = (TextAsset)Resources.Load(filename, typeof(TextAsset));

		Debug.Log("loading "+filename);

		reader = new StringReader(puzdata.text);
		if ( reader == null )
			Debug.Log(filename +" not found or not readable");
		else {
			string line;
			while ( (line = reader.ReadLine()) != null )
				entries.Add(line.Trim());
		}
		return entries;
	}

	public static bool LoadTextList(string filename, out string result) {
		StringReader reader = null; 
		result = "";

		TextAsset puzdata = (TextAsset)Resources.Load(filename, typeof(TextAsset));
		
		Debug.Log("LoadTextList: Loading "+filename);
		
		reader = new StringReader(puzdata.text);
		if ( reader == null ) {
			Debug.Log(filename +" not found or not readable");
			return false;
		} else {
			string line;
			while ( (line = reader.ReadLine()) != null )
				result = string.Concat(result,line.Trim());
		}
		return true;
	}


	public static string[] SplitCsvLine(string line)
	{
		string pattern = @"
						     (?!\s*$)                                      # Don't match empty last value.
						     \s*                                           # Strip whitespace before value.
						     (?:                                           # Group for value alternatives.
						       '(?<val>[^'\\]*(?:\\[\S\s][^'\\]*)*)'       # Either $1: Single quoted string,
						     | ""(?<val>[^""\\]*(?:\\[\S\s][^""\\]*)*)""   # or $2: Double quoted string,
						     | (?<val>[^,'""\s\\]*(?:\s+[^,'""\s\\]+)*)    # or $3: Non-comma, non-quote stuff.
						     )                                             # End group of value alternatives.
						     \s*                                           # Strip whitespace after value.
						     (?:,|$)                                       # Field ends on comma or EOS.
						     ";
		
//		string[] values = (from Match m in Regex.Matches(line, pattern, 
//		                                                 RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline)
//		                   select m.Groups[1].Value).ToArray();

//		return (from Match m in Regex.Matches(line, pattern, 
//											  RegexOptions.ExplicitCapture | 
//		                                      RegexOptions.IgnorePatternWhitespace | 
//		                                      RegexOptions.Multiline)
//				select m.Groups[1].Value).ToArray();

		return Regex.Matches(line, pattern, 
							RegexOptions.ExplicitCapture | 
							RegexOptions.IgnorePatternWhitespace | 
							RegexOptions.Multiline).OfType<Match>().Select(m => m.Groups[1].Value).ToArray();
	}
}
