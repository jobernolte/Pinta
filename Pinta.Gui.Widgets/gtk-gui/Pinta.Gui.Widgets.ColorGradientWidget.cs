
// This file has been generated by the GUI designer. Do not modify.
namespace Pinta.Gui.Widgets
{
	public partial class ColorGradientWidget
	{
		private global::Gtk.EventBox eventbox;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Pinta.Gui.Widgets.ColorGradientWidget
			global::Stetic.BinContainer.Attach (this);
			this.CanFocus = true;
			this.Events = ((global::Gdk.EventMask)(1534));
			this.Name = "Pinta.Gui.Widgets.ColorGradientWidget";
			// Container child Pinta.Gui.Widgets.ColorGradientWidget.Gtk.Container+ContainerChild
			this.eventbox = new global::Gtk.EventBox ();
			this.eventbox.Events = ((global::Gdk.EventMask)(790));
			this.eventbox.Name = "eventbox";
			this.eventbox.VisibleWindow = false;
			this.Add (this.eventbox);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Show ();
		}
	}
}
