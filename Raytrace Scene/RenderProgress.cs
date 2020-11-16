using System;

namespace Raytrace_Scene
{
	public class RenderProgress
	{
		public int LinesCompleted { get; private set; }
		public int TotalLines { get; private set; }
		private object lines_lock = new object();

		public event EventHandler<LineCompletedEventArgs> LineCompletedEvent;

		public RenderProgress(int total_lines)
		{
			TotalLines = total_lines;
			LinesCompleted = 0;
		}

		public void FinishLine()
		{
			lock (lines_lock)
			{
				LinesCompleted++;
				OnRaiseCustomEvent(new LineCompletedEventArgs(LinesCompleted, TotalLines));
			}
		}
		
		protected virtual void OnRaiseCustomEvent(LineCompletedEventArgs e)
		{
			EventHandler<LineCompletedEventArgs> raise_event = LineCompletedEvent;

			raise_event?.Invoke(this, e);
		}

		public class LineCompletedEventArgs : EventArgs
		{
			public int LinesCompleted { get; }
			public int TotalLines { get; }
			
			public LineCompletedEventArgs(int lines_completed, int total_lines)
			{
				LinesCompleted = lines_completed;
				TotalLines = total_lines;
			}
		}
	}
}