using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000580 RID: 1408
	public class Graphic_StackCount : Graphic_Collection
	{
		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x060029AB RID: 10667 RVA: 0x000FF325 File Offset: 0x000FD525
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[this.subGraphics.Length - 1].MatSingle;
			}
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x000FF33D File Offset: 0x000FD53D
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_StackCount>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x000FF35A File Offset: 0x000FD55A
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x000FF36D File Offset: 0x000FD56D
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x000FF385 File Offset: 0x000FD585
		public virtual Graphic SubGraphicFor(Thing thing)
		{
			return this.SubGraphicForStackCount(thing.stackCount, thing.def);
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x000FF39C File Offset: 0x000FD59C
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
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x000FF3D0 File Offset: 0x000FD5D0
		public Graphic SubGraphicForStackCount(int stackCount, ThingDef def)
		{
			switch (this.subGraphics.Length)
			{
			case 1:
				return this.subGraphics[0];
			case 2:
				if (stackCount == 1)
				{
					return this.subGraphics[0];
				}
				return this.subGraphics[1];
			case 3:
				if (stackCount == 1)
				{
					return this.subGraphics[0];
				}
				if (stackCount == def.stackLimit)
				{
					return this.subGraphics[2];
				}
				return this.subGraphics[1];
			default:
			{
				if (stackCount == 1)
				{
					return this.subGraphics[0];
				}
				if (stackCount == def.stackLimit)
				{
					return this.subGraphics[this.subGraphics.Length - 1];
				}
				int num = Mathf.Min(1 + Mathf.RoundToInt((float)stackCount / (float)def.stackLimit * ((float)this.subGraphics.Length - 3f) + 1E-05f), this.subGraphics.Length - 2);
				return this.subGraphics[num];
			}
			}
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x000FF4AC File Offset: 0x000FD6AC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"StackCount(path=",
				this.path,
				", count=",
				this.subGraphics.Length.ToString(),
				")"
			});
		}
	}
}
