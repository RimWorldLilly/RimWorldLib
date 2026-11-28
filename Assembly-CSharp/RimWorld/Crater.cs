using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B22 RID: 6946
	public class Crater : Building
	{
		// Token: 0x17002014 RID: 8212
		// (get) Token: 0x0600B1F9 RID: 45561 RVA: 0x003EAE5C File Offset: 0x003E905C
		protected virtual EffecterDef FilledInEffecter
		{
			get
			{
				return EffecterDefOf.ImpactSmallDustCloud;
			}
		}

		// Token: 0x17002015 RID: 8213
		// (get) Token: 0x0600B1FA RID: 45562 RVA: 0x003EAE63 File Offset: 0x003E9063
		protected virtual SoundDef FilledInSound
		{
			get
			{
				return SoundDefOf.Crater_FilledIn;
			}
		}

		// Token: 0x0600B1FB RID: 45563 RVA: 0x003EAE6C File Offset: 0x003E906C
		public virtual void FillIn()
		{
			EffecterDef filledInEffecter = this.FilledInEffecter;
			if (filledInEffecter != null)
			{
				filledInEffecter.Spawn(base.Position, base.Map, 1f).Cleanup();
			}
			SoundDef filledInSound = this.FilledInSound;
			if (filledInSound != null)
			{
				filledInSound.PlayOneShot(this);
			}
			this.Destroy(DestroyMode.Deconstruct);
		}

		// Token: 0x0600B1FC RID: 45564 RVA: 0x003EAEBE File Offset: 0x003E90BE
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Crater.<GetGizmos>d__5 <GetGizmos>d__ = new Crater.<GetGizmos>d__5(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}
	}
}
