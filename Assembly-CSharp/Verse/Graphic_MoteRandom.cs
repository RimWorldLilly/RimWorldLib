using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000572 RID: 1394
	[StaticConstructorOnStartup]
	public class Graphic_MoteRandom : Graphic_Random
	{
		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06002950 RID: 10576 RVA: 0x000FDAAA File Offset: 0x000FBCAA
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x000FDAB0 File Offset: 0x000FBCB0
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic_Mote.DrawMote(this.data, this.SubGraphicFor((Mote)thing).MatSingle, base.Color, loc, rot, thingDef, thing, 0, this.ForcePropertyBlock, null);
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x000FDAED File Offset: 0x000FBCED
		public Graphic SubGraphicFor(Mote mote)
		{
			return this.subGraphics[mote.offsetRandom % this.subGraphics.Length];
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x000FDB08 File Offset: 0x000FBD08
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "Mote(path=";
			array[1] = this.path;
			array[2] = ", shader=";
			int num = 3;
			Shader shader = base.Shader;
			array[num] = ((shader != null) ? shader.ToString() : null);
			array[4] = ", color=";
			array[5] = this.color.ToString();
			array[6] = ", colorTwo=unsupported)";
			return string.Concat(array);
		}

		// Token: 0x04001CD1 RID: 7377
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
