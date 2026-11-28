using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023AD RID: 9133
	public class EscapeShip : MapParent
	{
		// Token: 0x170027F0 RID: 10224
		// (get) Token: 0x0600E35F RID: 58207 RVA: 0x00549280 File Offset: 0x00547480
		public override Texture2D ExpandingIcon
		{
			get
			{
				if (!base.HasMap || base.Faction == null)
				{
					return base.ExpandingIcon;
				}
				return base.Faction.def.FactionIcon;
			}
		}

		// Token: 0x170027F1 RID: 10225
		// (get) Token: 0x0600E360 RID: 58208 RVA: 0x005492A9 File Offset: 0x005474A9
		public override Color ExpandingIconColor
		{
			get
			{
				if (!base.HasMap || base.Faction == null)
				{
					return base.ExpandingIconColor;
				}
				return base.Faction.Color;
			}
		}

		// Token: 0x170027F2 RID: 10226
		// (get) Token: 0x0600E361 RID: 58209 RVA: 0x005492D0 File Offset: 0x005474D0
		public override Material Material
		{
			get
			{
				if (!base.HasMap || base.Faction == null)
				{
					return base.Material;
				}
				if (this.cachedPostGenerateMat == null)
				{
					this.cachedPostGenerateMat = MaterialPool.MatFrom(base.Faction.def.settlementTexturePath, ShaderDatabase.WorldOverlayTransparentLit, base.Faction.Color, 3550);
				}
				return this.cachedPostGenerateMat;
			}
		}

		// Token: 0x0600E362 RID: 58210 RVA: 0x00549338 File Offset: 0x00547538
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			Find.World.renderer.SetDirty<WorldDrawLayer_WorldObjects>(base.Tile.Layer);
		}

		// Token: 0x04009100 RID: 37120
		private Material cachedPostGenerateMat;
	}
}
