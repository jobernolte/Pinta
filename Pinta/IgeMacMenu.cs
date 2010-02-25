// 
// IgeMacMenu.cs
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


	public class IgeMacMenu
	{
		[DllImport("libdl")]
		static extern IntPtr dlopen (string filename, int flags);
		[DllImport("libdl")]
		static extern IntPtr dlsym (IntPtr handle, string symbol);

		private static IntPtr lib;

		static IgeMacMenu ()
		{
			// try to load the libray first using the given path enviroment
			lib = dlopen ("libigemacintegration.dylib", 0);
			if (lib == IntPtr.Zero) {
				// load the library from an absolute path. not so fine, but should work.
				lib = dlopen ("/Library/Frameworks/Mono.framework/Versions/Current/lib/libigemacintegration.dylib", 0);
			}
		}

		[DllImport("libigemacintegration.dylib")]
		static extern void ige_mac_menu_connect_window_key_handler (IntPtr window);

		public static void ConnectWindowKeyHandler (Gtk.Window window)
		{
			ige_mac_menu_connect_window_key_handler (window.Handle);
		}

		[DllImport("libigemacintegration.dylib")]
		static extern void ige_mac_menu_set_global_key_handler_enabled_imported (bool enabled);

		internal delegate void ige_mac_menu_set_global_key_handler_enabled (bool enabled);

		public static bool GlobalKeyHandlerEnabled {
			set {
				IntPtr procaddr = dlsym (lib, "ige_mac_menu_set_global_key_handler_enabled");
				if (procaddr != IntPtr.Zero) {
					ige_mac_menu_set_global_key_handler_enabled func = (ige_mac_menu_set_global_key_handler_enabled)Marshal.GetDelegateForFunctionPointer (procaddr, typeof(ige_mac_menu_set_global_key_handler_enabled));
					func (value);
				} else {
					ige_mac_menu_set_global_key_handler_enabled_imported (value);
				}
			}
		}

		[DllImport("libigemacintegration.dylib")]
		static extern void ige_mac_menu_set_menu_bar_imported (IntPtr menu_shell);
		internal delegate void ige_mac_menu_set_menu_bar (IntPtr menu_shell);

		public static Gtk.MenuShell MenuBar {
			set {
				IntPtr procaddr = dlsym (lib, "ige_mac_menu_set_menu_bar");
				if (procaddr != IntPtr.Zero) {
					ige_mac_menu_set_menu_bar func = (ige_mac_menu_set_menu_bar)Marshal.GetDelegateForFunctionPointer (procaddr, typeof(ige_mac_menu_set_menu_bar));
					func (value == null ? IntPtr.Zero : value.Handle);
				} else {
					ige_mac_menu_set_menu_bar_imported (value == null ? IntPtr.Zero : value.Handle);
				}
			}
		}

		[DllImport("libigemacintegration.dylib")]
		static extern void ige_mac_menu_set_quit_menu_item (IntPtr quit_item);

		public static Gtk.MenuItem QuitMenuItem {
			set { ige_mac_menu_set_quit_menu_item (value == null ? IntPtr.Zero : value.Handle); }
		}

		[DllImport("libigemacintegration.dylib")]
		static extern IntPtr ige_mac_menu_add_app_menu_group_imported ();
		internal delegate IntPtr ige_mac_menu_add_app_menu_group ();

		public static Pinta.IgeMacMenuGroup AddAppMenuGroup ()
		{
			IntPtr raw_ret;
			
			IntPtr procaddr = dlsym (lib, "ige_mac_menu_add_app_menu_group");
			if (procaddr != IntPtr.Zero) {
				ige_mac_menu_add_app_menu_group func = (ige_mac_menu_add_app_menu_group)Marshal.GetDelegateForFunctionPointer (procaddr, typeof(ige_mac_menu_add_app_menu_group));
				raw_ret = func ();
			} else {
				raw_ret = ige_mac_menu_add_app_menu_group_imported ();
			}
			
			Pinta.IgeMacMenuGroup ret = raw_ret == IntPtr.Zero ? null : (Pinta.IgeMacMenuGroup)GLib.Opaque.GetOpaque (raw_ret, typeof(Pinta.IgeMacMenuGroup), false);
			ret.Lib = lib;
			return ret;
		}
		
	}

	public class IgeMacMenuGroup : GLib.Opaque
	{
		[DllImport("libdl")]
		static extern IntPtr dlsym (IntPtr handle, string symbol);

		private IntPtr lib;

		[DllImport("libigemacintegration.dylib")]
		static extern void ige_mac_menu_add_app_menu_item_imported (IntPtr raw, IntPtr menu_item, IntPtr label);
		internal delegate void ige_mac_menu_add_app_menu_item (IntPtr raw, IntPtr menu_item, IntPtr label);

		public void AddMenuItem (Gtk.MenuItem menu_item, string label)
		{
			IntPtr native_label = GLib.Marshaller.StringToPtrGStrdup (label);
			
			IntPtr procaddr = dlsym (lib, "ige_mac_menu_add_app_menu_item");
			if (procaddr != IntPtr.Zero) {
				ige_mac_menu_add_app_menu_item func = (ige_mac_menu_add_app_menu_item)Marshal.GetDelegateForFunctionPointer (procaddr, typeof(ige_mac_menu_add_app_menu_item));
				func (Handle, menu_item == null ? IntPtr.Zero : menu_item.Handle, native_label);
			} else {
				ige_mac_menu_add_app_menu_item_imported (Handle, menu_item == null ? IntPtr.Zero : menu_item.Handle, native_label);
			}
			
			GLib.Marshaller.Free (native_label);
		}

		public IgeMacMenuGroup (IntPtr raw) : base(raw)
		{
		}

		public IntPtr Lib {
			get { return this.lib; }
			set { this.lib = value; }
		}
	}
}
