// 
// Platform.cs
//  
// Author:
//       jurgenobernolte <${juergen.obernolte@arcor.de}>
// 
// Copyright (c) 2010 jurgenobernolte
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Runtime.InteropServices; 

namespace Pinta
{


	public static class Platform
	{
		[DllImport("libc")]
		static extern int uname (IntPtr buf);
		[DllImport("libdl")]
		static extern IntPtr dlopen(string filename, int flags);
		
		private static bool isWindows;
		private static bool isMac;
		private static bool isX11;
		
		public enum OS
		{
			Windows,
			Mac,
			X11,
			Unknown
		}
		
		static Platform() 
		{
			isWindows = System.IO.Path.DirectorySeparatorChar == '\\';
			isMac = !isWindows && IsRunningOnMac ();
			isX11 = !isMac && System.Environment.OSVersion.Platform == PlatformID.Unix;
		}
		
		public static OS GetOS ()
		{
			if (isWindows)
				return OS.Windows;
			if (isMac)
				return OS.Mac;
			if (isX11)
				return OS.X11;
			return OS.Unknown;
		}
		
		//From Managed.Windows.Forms/XplatUI
		static bool IsRunningOnMac ()
		{
			IntPtr buf = IntPtr.Zero;
			try {
				buf = Marshal.AllocHGlobal (8192);
				// This is a hacktastic way of getting sysname from uname ()
				if (uname (buf) == 0) {
					string os = Marshal.PtrToStringAnsi (buf);
					if (os == "Darwin")
						return true;
				}
			} catch {
			} finally {
				if (buf != IntPtr.Zero)
					Marshal.FreeHGlobal (buf);
			}
			return false;
		}
	
		public static IntPtr LoadLibrary(string filename)
		{
			return dlopen(filename, 0);
		}
	}
}
