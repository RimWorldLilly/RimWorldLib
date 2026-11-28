using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000559 RID: 1369
	[StaticConstructorOnStartup]
	public class GraphicMote_RandomWithAgeSecs : Graphic_Random
	{
		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x060028C9 RID: 10441 RVA: 0x000FB2EE File Offset: 0x000F94EE
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x000FB2F4 File Offset: 0x000F94F4
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mote mote = (Mote)thing;
			GraphicMote_RandomWithAgeSecs.propertyBlock.SetColor(ShaderPropertyIDs.Color, this.color);
			GraphicMote_RandomWithAgeSecs.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, mote.AgeSecs);
			GraphicMote_RandomWithAgeSecs.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecsPausable, mote.AgeSecsPausable);
			Graphic_Mote.DrawMote(this.data, this.SubGraphicFor((Mote)thing).MatSingle, base.Color, loc, rot, thingDef, thing, 0, this.ForcePropertyBlock, GraphicMote_RandomWithAgeSecs.propertyBlock);
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x000FB37C File Offset: 0x000F957C
		public Graphic SubGraphicFor(Mote mote)
		{
			return this.subGraphics[mote.offsetRandom % this.subGraphics.Length];
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x000FB394 File Offset: 0x000F9594
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

		// Token: 0x04001CB4 RID: 7348
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
