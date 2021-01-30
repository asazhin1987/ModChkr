//using System;
//using System.IO;
//using System.Reflection;

//namespace ModelChecker
//{
//	public static class BTextWriter
//	{
//		public static void WriteLog(string txt, string path = null)
//		{
//			using (StreamWriter file = new StreamWriter(path ?? Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Log.txt"), true))
//			{
//				file.WriteLineAsync($"{DateTime.Now}\t{txt}");
//			}
//		}

//		public static string ReadFile(string path)
//		{
//			if (File.Exists(path))
//			{
//				return File.ReadAllText(path);
//			}
//			return "";
//		}

//		public static string ReadCurrentFile(string name)
//		{
//			string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
//			if (File.Exists(path))
//			{
//				return File.ReadAllText(path);
//			}
//			return "";
//		}

//		public static void WriteCurrentFile(string txt, string name, bool rewrite = true)
//		{
//			string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
//			using (StreamWriter file = new StreamWriter(path, !rewrite))
//			{
//				file.Write(txt);
//			}
//		}
//	}
//}
