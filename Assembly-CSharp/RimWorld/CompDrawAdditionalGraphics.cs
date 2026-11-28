using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001869 RID: 6249
	public class CompDrawAdditionalGraphics : ThingComp
	{
		// Token: 0x17001AB8 RID: 6840
		// (get) Token: 0x06009C52 RID: 40018 RVA: 0x003846C2 File Offset: 0x003828C2
		private CompProperties_DrawAdditionalGraphics Props
		{
			get
			{
				return (CompProperties_DrawAdditionalGraphics)this.props;
			}
		}

		// Token: 0x06009C53 RID: 40019 RVA: 0x003846D0 File Offset: 0x003828D0
		public override void PostDraw()
		{
			foreach (GraphicData graphicData in this.Props.graphics)
			{
				graphicData.Graphic.Draw(this.parent.DrawPos, this.parent.Rotation, this.parent, 0f);
			}
		}
	}
}
