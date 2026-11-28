using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200057B RID: 1403
	public class Graphic_RandomRotated : Graphic
	{
		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06002988 RID: 10632 RVA: 0x000FEC3C File Offset: 0x000FCE3C
		public override Material MatSingle
		{
			get
			{
				return this.subGraphic.MatSingle;
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06002989 RID: 10633 RVA: 0x000FEC49 File Offset: 0x000FCE49
		public Graphic SubGraphic
		{
			get
			{
				return this.subGraphic;
			}
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x000FEC51 File Offset: 0x000FCE51
		public Graphic_RandomRotated(Graphic subGraphic, float maxAngle)
		{
			this.subGraphic = subGraphic;
			this.maxAngle = maxAngle;
			this.drawSize = subGraphic.drawSize;
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x000FEC73 File Offset: 0x000FCE73
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.subGraphic.MatAt(rot, thing);
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x000FEC82 File Offset: 0x000FCE82
		public override Material MatSingleFor(Thing thing)
		{
			return this.subGraphic.MatSingleFor(thing);
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x000FEC90 File Offset: 0x000FCE90
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mesh mesh = this.MeshAt(rot);
			float num = 0f;
			float? rotInRack = this.GetRotInRack(thing, thingDef, loc.ToIntVec3());
			if (rotInRack != null)
			{
				num = rotInRack.Value;
			}
			else if (thing != null)
			{
				num = -this.maxAngle + (float)(thing.thingIDNumber * 542) % (this.maxAngle * 2f);
			}
			num += extraRotation;
			Graphics.DrawMesh(mesh, loc, Quaternion.AngleAxis(num, Vector3.up), this.MatSingleFor(thing), 0, null, 0);
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x000FED16 File Offset: 0x000FCF16
		public override string ToString()
		{
			string str = "RandomRotated(subGraphic=";
			Graphic graphic = this.subGraphic;
			return str + ((graphic != null) ? graphic.ToString() : null) + ")";
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x000FED39 File Offset: 0x000FCF39
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_RandomRotated(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo), this.maxAngle)
			{
				data = this.data,
				drawSize = this.drawSize
			};
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x000FED6C File Offset: 0x000FCF6C
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			float num = 0f;
			float? rotInRack = this.GetRotInRack(thing, thing.def, thing.Position);
			if (rotInRack != null)
			{
				num = rotInRack.Value;
			}
			else if (thing != null)
			{
				num = -this.maxAngle + (float)(thing.thingIDNumber * 542) % (this.maxAngle * 2f);
			}
			num += extraRotation;
			this.subGraphic.Print(layer, thing, num);
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x000FEDDE File Offset: 0x000FCFDE
		public override void TryInsertIntoAtlas(TextureAtlasGroup group)
		{
			this.subGraphic.TryInsertIntoAtlas(group);
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x000FEDEC File Offset: 0x000FCFEC
		private float? GetRotInRack(Thing thing, ThingDef thingDef, IntVec3 loc)
		{
			if (thing == null || !thingDef.IsWeapon || !thing.Spawned || !loc.InBounds(thing.Map) || loc.GetEdifice(thing.Map) == null || loc.GetItemCount(thing.Map) < 2)
			{
				return null;
			}
			if (thingDef.rotateInShelves)
			{
				return new float?(-90f);
			}
			return new float?(0f);
		}

		// Token: 0x04001CE0 RID: 7392
		private Graphic subGraphic;

		// Token: 0x04001CE1 RID: 7393
		private float maxAngle;
	}
}
