using System;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000050 RID: 80
	public abstract class Window_Dev : Window
	{
		// Token: 0x0600021B RID: 539 RVA: 0x0000FE84 File Offset: 0x0000E084
		public Window_Dev() : base(new DevWindowDrawing())
		{
		}
	}
}
