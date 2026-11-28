using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200057A RID: 1402
	public class Graphic_Random : Graphic_Collection
	{
		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x0600297C RID: 10620 RVA: 0x000FEA7C File Offset: 0x000FCC7C
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x0600297D RID: 10621 RVA: 0x000FEA98 File Offset: 0x000FCC98
		public int SubGraphicsCount
		{
			get
			{
				return this.subGraphics.Length;
			}
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x000FEAA2 File Offset: 0x000FCCA2
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Random.GetColoredVersion with a non-white colorTwo.", 9910251);
			}
			return GraphicDatabase.Get<Graphic_Random>(this.path, newShader, this.drawSize, newColor, Color.white, this.data, null);
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x000FEADF File Offset: 0x000FCCDF
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x000FEAF2 File Offset: 0x000FCCF2
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x000FEB0A File Offset: 0x000FCD0A
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			((thing != null) ? this.SubGraphicFor(thing) : this.subGraphics[0]).DrawWorker(loc, rot, thingDef, thing, extraRotation);
			if (base.ShadowGraphic != null)
			{
				base.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x000FEB48 File Offset: 0x000FCD48
		public Graphic SubGraphicFor(Thing thing)
		{
			if (thing == null)
			{
				return this.subGraphics[0];
			}
			int num = thing.OverrideGraphicIndex ?? thing.thingIDNumber;
			return this.subGraphics[num % this.subGraphics.Length];
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x000FEB92 File Offset: 0x000FCD92
		public Graphic SubGraphicAtIndex(int index)
		{
			return this.subGraphics[index % this.subGraphics.Length];
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000FEBA5 File Offset: 0x000FCDA5
		public Graphic FirstSubgraphic()
		{
			return this.subGraphics[0];
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x000FEBAF File Offset: 0x000FCDAF
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			((thing != null) ? this.SubGraphicFor(thing) : this.subGraphics[0]).Print(layer, thing, extraRotation);
			if (base.ShadowGraphic != null && thing != null)
			{
				base.ShadowGraphic.Print(layer, thing, extraRotation);
			}
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x000FEBE8 File Offset: 0x000FCDE8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Random(path=",
				this.path,
				", count=",
				this.subGraphics.Length.ToString(),
				")"
			});
		}
	}
}
