using System;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200055D RID: 1373
	public class Graphic_ActivityStaged : Graphic_Collection
	{
		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x060028D7 RID: 10455 RVA: 0x000FB6E0 File Offset: 0x000F98E0
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[0].MatSingle;
			}
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x000FB6EF File Offset: 0x000F98EF
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_ActivityStaged>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x000FB70C File Offset: 0x000F990C
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x000FB71F File Offset: 0x000F991F
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000FB737 File Offset: 0x000F9937
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			((thing != null) ? this.SubGraphicFor(thing) : this.subGraphics[0]).DrawWorker(loc, rot, thingDef, thing, extraRotation);
			if (base.ShadowGraphic != null)
			{
				base.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000FB775 File Offset: 0x000F9975
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			((thing != null) ? this.SubGraphicFor(thing) : this.subGraphics[0]).Print(layer, thing, extraRotation);
			if (base.ShadowGraphic != null && thing != null)
			{
				base.ShadowGraphic.Print(layer, thing, extraRotation);
			}
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x000FB7AC File Offset: 0x000F99AC
		private Graphic SubGraphicFor(Thing thing)
		{
			if (thing == null)
			{
				return this.subGraphics[0];
			}
			CompActivity compActivity;
			if (!thing.TryGetComp(out compActivity))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 3);
				defaultInterpolatedStringHandler.AppendFormatted(thing.Label);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted("Graphic_ActivityStaged");
				defaultInterpolatedStringHandler.AppendLiteral(" requires ");
				defaultInterpolatedStringHandler.AppendFormatted("CompActivity");
				defaultInterpolatedStringHandler.AppendLiteral(".");
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 4627811);
				return null;
			}
			int num = Mathf.Min(Mathf.FloorToInt((float)this.subGraphics.Length * compActivity.ActivityLevel), this.subGraphics.Length - 1);
			return this.subGraphics[num];
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x000FB864 File Offset: 0x000F9A64
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"ActivityStaged(path=",
				this.path,
				", count=",
				this.subGraphics.Length.ToString(),
				")"
			});
		}
	}
}
