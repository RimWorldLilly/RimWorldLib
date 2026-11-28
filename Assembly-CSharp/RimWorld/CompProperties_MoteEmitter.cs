using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200190E RID: 6414
	public class CompProperties_MoteEmitter : CompProperties
	{
		// Token: 0x0600A0C9 RID: 41161 RVA: 0x00397DB4 File Offset: 0x00395FB4
		public CompProperties_MoteEmitter()
		{
			this.compClass = typeof(CompMoteEmitter);
		}

		// Token: 0x17001BB9 RID: 7097
		// (get) Token: 0x0600A0CA RID: 41162 RVA: 0x00397E20 File Offset: 0x00396020
		public Vector3 EmissionOffset
		{
			get
			{
				return new Vector3(Rand.Range(this.offsetMin.x, this.offsetMax.x), Rand.Range(this.offsetMin.y, this.offsetMax.y), Rand.Range(this.offsetMin.z, this.offsetMax.z));
			}
		}

		// Token: 0x0600A0CB RID: 41163 RVA: 0x00397E84 File Offset: 0x00396084
		public Vector3 RotationOffset(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return this.offsetNorth;
			case 1:
				return this.offsetEast;
			case 2:
				return this.offsetSouth;
			case 3:
				return this.offsetWest;
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x0600A0CC RID: 41164 RVA: 0x00397ED2 File Offset: 0x003960D2
		public ThingDef RotationMote(Rot4 rot)
		{
			List<ThingDef> list = this.perRotationMotes;
			if (list == null)
			{
				return null;
			}
			return list[rot.AsInt];
		}

		// Token: 0x0600A0CD RID: 41165 RVA: 0x00397EEC File Offset: 0x003960EC
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_MoteEmitter.<ConfigErrors>d__20 <ConfigErrors>d__ = new CompProperties_MoteEmitter.<ConfigErrors>d__20(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005EE5 RID: 24293
		public ThingDef mote;

		// Token: 0x04005EE6 RID: 24294
		public List<ThingDef> perRotationMotes;

		// Token: 0x04005EE7 RID: 24295
		public Vector3 offset;

		// Token: 0x04005EE8 RID: 24296
		public Vector3 offsetMin = Vector3.zero;

		// Token: 0x04005EE9 RID: 24297
		public Vector3 offsetMax = Vector3.zero;

		// Token: 0x04005EEA RID: 24298
		public Vector3 offsetNorth = Vector3.zero;

		// Token: 0x04005EEB RID: 24299
		public Vector3 offsetSouth = Vector3.zero;

		// Token: 0x04005EEC RID: 24300
		public Vector3 offsetEast = Vector3.zero;

		// Token: 0x04005EED RID: 24301
		public Vector3 offsetWest = Vector3.zero;

		// Token: 0x04005EEE RID: 24302
		public bool useParentRotation;

		// Token: 0x04005EEF RID: 24303
		public SoundDef soundOnEmission;

		// Token: 0x04005EF0 RID: 24304
		public int emissionInterval = -1;

		// Token: 0x04005EF1 RID: 24305
		public int ticksSinceLastEmittedMaxOffset;

		// Token: 0x04005EF2 RID: 24306
		public bool maintain;

		// Token: 0x04005EF3 RID: 24307
		[NoTranslate]
		public string saveKeysPrefix;
	}
}
