using System;
using System.Diagnostics;
using System.IO;

namespace FLAC2ALAC
{
	class Program
	{
		static int Main(string[] args)
		{
			int count = 0;
			if (args.Length == 0)
			{
				Console.WriteLine("当前未读取到文件，请将需要转换的文件拖拽到本程序图标上进行转换。");
				Console.ReadLine();
				return 0;
			}
			foreach (string fullFileName in args)
			{
				string filePathWithoutExt = fullFileName.Replace(".flac", "");
				if (GetCover(filePathWithoutExt))
				{
					if (FLAC2ALAC(filePathWithoutExt))
					{
						//PutCover(filePathWithoutExt);
						//File.Delete(filePathWithoutExt + "_tmp.m4a");
						//File.Delete(filePathWithoutExt + ".jpg");
						Console.WriteLine("已完成[" + filePathWithoutExt + ".flac]的转换");
						count++;
					}
				}
			}
			Console.WriteLine("所有转换任务已完成，共处理" + count + "/" + args.Length.ToString() + "个项目");
			Console.ReadLine();
			return 0;
		}

		private static bool FLAC2ALAC(string filepath)
		{
			var flac2alac = new Process();
			flac2alac.StartInfo.FileName = "ffmpeg.exe";
			flac2alac.StartInfo.Arguments = "-i \"" + filepath + ".flac\" -sn -vn -acodec alac \"" + filepath + ".m4a\" -y";
			flac2alac.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			try
			{
				flac2alac.Start();
				int try_times = 0;
				while (!flac2alac.WaitForExit(10000))
				{
					try_times++;
					if (try_times > 3)
					{
						flac2alac.Kill();
						return false;
					}
					continue;
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		private static bool GetCover(string filepath)
		{
			var getCover = new Process();
			getCover.StartInfo.FileName = "ffmpeg.exe";
			getCover.StartInfo.Arguments = "-i \"" + filepath + ".flac\" \"" + filepath + ".jpg\" -y";
			getCover.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			try
			{
				getCover.Start();
				int try_times = 0;
				while (!getCover.WaitForExit(10000))
				{
					try_times++;
					if (try_times > 3)
					{
						getCover.Kill();
						return false;
					}
					continue;
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		private static bool PutCover(string filepath)
		{
			var putCover = new Process();
			putCover.StartInfo.FileName = "ffmpeg.exe";
			putCover.StartInfo.Arguments = "-i \"" + filepath + "_tmp.m4a\" -i \"" + filepath + ".jpg\" -map 0 -map 1 -c copy -disposition:v:0 attached_pic \"" + filepath + ".m4a\"";
			putCover.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			try
			{
				putCover.Start();
				int try_times = 0;
				while (!putCover.WaitForExit(10000))
				{
					try_times++;
					if (try_times > 3)
					{
						putCover.Kill();
						return false;
					}
					continue;
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}
	}
}
