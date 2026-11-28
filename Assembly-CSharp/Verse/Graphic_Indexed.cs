using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200056A RID: 1386
	public class Graphic_Indexed : Graphic_Collection
	{
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x0600291D RID: 10525 RVA: 0x000FCBEF File Offset: 0x000FADEF
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[0].MatSingle;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x0600291E RID: 10526 RVA: 0x000FCBFE File Offset: 0x000FADFE
		public int SubGraphicsCount
		{
			get
			{
				return this.subGraphics.Length;
			}
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x000FCC08 File Offset: 0x000FAE08
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Indexed.GetColoredVersion with a non-white colorTwo.", 9910251);
			}
			return GraphicDatabase.Get<Graphic_Indexed>(this.path, newShader, this.drawSize, newColor, Color.white, this.data, null);
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x000FCC45 File Offset: 0x000FAE45
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x000FCC58 File Offset: 0x000FAE58
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x000FCC70 File Offset: 0x000FAE70
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic graphic;
			if (thing != null)
			{
				graphic = this.SubGraphicFor(thing);
			}
			else
			{
				graphic = this.subGraphics[0];
			}
			graphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			if (base.ShadowGraphic != null)
			{
				base.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x000FCCBC File Offset: 0x000FAEBC
		public Graphic SubGraphicFor(Thing thing)
		{
			if (thing == null)
			{
				return this.subGraphics[0];
			}
			int valueOrDefault = thing.OverrideGraphicIndex.GetValueOrDefault();
			return this.subGraphics[valueOrDefault % this.subGraphics.Length];
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x000FCCF5 File Offset: 0x000FAEF5
		public Graphic SubGraphicAtIndex(int index)
		{
			return this.subGraphics[index % this.subGraphics.Length];
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x000FCD08 File Offset: 0x000FAF08
		public Graphic FirstSubgraphic()
		{
			return this.subGraphics[0];
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x000FCD14 File Offset: 0x000FAF14
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Indexed(path=",
				this.path,
				", count=",
				this.subGraphics.Length.ToString(),
				")"
			});
		}
	}
}
