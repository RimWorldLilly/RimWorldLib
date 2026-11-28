using System;

namespace Verse
{
	// Token: 0x020002B2 RID: 690
	public abstract class SectionLayer_Dynamic : SectionLayer
	{
		// Token: 0x06001521 RID: 5409 RVA: 0x0007BDDA File Offset: 0x00079FDA
		public virtual bool ShouldDrawDynamic(CellRect view)
		{
			return true;
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x0007BDDD File Offset: 0x00079FDD
		public SectionLayer_Dynamic(Section section) : base(section)
		{
			this.section = section;
		}
	}
}
