using System;

namespace LudeonTK
{
	// Token: 0x0200003A RID: 58
	public static class DebugTools
	{
		// Token: 0x0600018B RID: 395 RVA: 0x0000B57B File Offset: 0x0000977B
		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
			if (DebugTools.curMeasureTool != null)
			{
				DebugTools.curMeasureTool.DebugToolOnGUI();
			}
		}

		// Token: 0x040000AB RID: 171
		public static DebugTool curTool;

		// Token: 0x040000AC RID: 172
		public static MeasureTool curMeasureTool;
	}
}
