using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000574 RID: 1396
	public class Graphic_MoteWithParentRotation : Graphic_Mote
	{
		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x0600295A RID: 10586 RVA: 0x000FDD8A File Offset: 0x000FBF8A
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x000FDD90 File Offset: 0x000FBF90
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			MoteAttached moteAttached = (MoteAttached)thing;
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, this.color);
			if (moteAttached != null && moteAttached.link1.Linked)
			{
				Graphic_Mote.propertyBlock.SetInt(ShaderPropertyIDs.Rotation, moteAttached.link1.Target.Thing.Rotation.AsInt);
			}
			base.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x000FDE08 File Offset: 0x000FC008
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "Graphic_MoteWithParentRotation(path=";
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
	}
}
