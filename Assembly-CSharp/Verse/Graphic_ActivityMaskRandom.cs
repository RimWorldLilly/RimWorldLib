using System;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200055C RID: 1372
	public class Graphic_ActivityMaskRandom : Graphic_WithPropertyBlockRandom
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x060028D4 RID: 10452 RVA: 0x000FB618 File Offset: 0x000F9818
		protected override Type SingleGraphicType
		{
			get
			{
				return typeof(Graphic_ActivityMask);
			}
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x000FB624 File Offset: 0x000F9824
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			CompActivity compActivity = thing.TryGetComp<CompActivity>();
			if (compActivity == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 3);
				defaultInterpolatedStringHandler.AppendFormatted(thingDef.defName);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted("Graphic_ActivityMask");
				defaultInterpolatedStringHandler.AppendLiteral(" requires ");
				defaultInterpolatedStringHandler.AppendFormatted("CompActivity");
				defaultInterpolatedStringHandler.AppendLiteral(".");
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 6134621);
				return;
			}
			Color colorTwo = this.colorTwo;
			colorTwo.a = Mathf.Clamp01(compActivity.ActivityLevel);
			this.propertyBlock.SetColor(ShaderPropertyIDs.ColorTwo, colorTwo);
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}
	}
}
