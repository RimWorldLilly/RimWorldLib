using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200188B RID: 6283
	public class CompFleshmassBase : ThingComp
	{
		// Token: 0x17001AE9 RID: 6889
		// (get) Token: 0x06009D3A RID: 40250 RVA: 0x00388730 File Offset: 0x00386930
		private CompProperties_FleshmassBase Props
		{
			get
			{
				return (CompProperties_FleshmassBase)this.props;
			}
		}

		// Token: 0x17001AEA RID: 6890
		// (get) Token: 0x06009D3B RID: 40251 RVA: 0x00388740 File Offset: 0x00386940
		public List<Graphic> CachedBaseGFX
		{
			get
			{
				if (CompFleshmassBase.cachedBaseGFX == null)
				{
					CompFleshmassBase.cachedBaseGFX = new List<Graphic>
					{
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_1x1A", ShaderDatabase.Cutout, new Vector2(2.3f, 2.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_1x1B", ShaderDatabase.Cutout, new Vector2(2.3f, 2.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_2x2A", ShaderDatabase.Cutout, new Vector2(3.3f, 3.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_2x2B", ShaderDatabase.Cutout, new Vector2(3.3f, 3.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_3x3A", ShaderDatabase.Cutout, new Vector2(4.3f, 4.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_3x3B", ShaderDatabase.Cutout, new Vector2(4.3f, 4.3f), Color.white)
					};
				}
				return CompFleshmassBase.cachedBaseGFX;
			}
		}

		// Token: 0x17001AEB RID: 6891
		// (get) Token: 0x06009D3C RID: 40252 RVA: 0x0038885C File Offset: 0x00386A5C
		public List<Graphic> CachedOutlineGFX
		{
			get
			{
				if (CompFleshmassBase.cachedOutlineGFX == null)
				{
					CompFleshmassBase.cachedOutlineGFX = new List<Graphic>
					{
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_1x1A_Outline", ShaderDatabase.Cutout, new Vector2(2.3f, 2.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_1x1B_Outline", ShaderDatabase.Cutout, new Vector2(2.3f, 2.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_2x2A_Outline", ShaderDatabase.Cutout, new Vector2(3.3f, 3.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_2x2B_Outline", ShaderDatabase.Cutout, new Vector2(3.3f, 3.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_3x3A_Outline", ShaderDatabase.Cutout, new Vector2(4.3f, 4.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_3x3B_Outline", ShaderDatabase.Cutout, new Vector2(4.3f, 4.3f), Color.white)
					};
				}
				return CompFleshmassBase.cachedOutlineGFX;
			}
		}

		// Token: 0x06009D3D RID: 40253 RVA: 0x00388978 File Offset: 0x00386B78
		public override void PostDraw()
		{
			int index = (this.Props.size - 1) * 2 + this.parent.thingIDNumber % 2;
			Rot4 rot = new Rot4(this.parent.thingIDNumber % 8 / 2);
			this.CachedBaseGFX[index].Draw(this.parent.DrawPos.WithYOffset(0.01f), rot, this.parent, 0f);
			this.CachedOutlineGFX[index].Draw(this.parent.DrawPos.WithYOffset(-0.01f), rot, this.parent, 0f);
		}

		// Token: 0x04005D2C RID: 23852
		[Unsaved(false)]
		private static List<Graphic> cachedBaseGFX;

		// Token: 0x04005D2D RID: 23853
		[Unsaved(false)]
		private static List<Graphic> cachedOutlineGFX;
	}
}
