using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001736 RID: 5942
	public class CompRitualEffect_ConstantCircle : CompRitualEffect_Constant
	{
		// Token: 0x06009421 RID: 37921 RVA: 0x003601DE File Offset: 0x0035E3DE
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x17001957 RID: 6487
		// (get) Token: 0x06009422 RID: 37922 RVA: 0x003601E5 File Offset: 0x0035E3E5
		protected CompProperties_RitualEffectConstantCircle Props
		{
			get
			{
				return (CompProperties_RitualEffectConstantCircle)this.props;
			}
		}

		// Token: 0x06009423 RID: 37923 RVA: 0x003601F2 File Offset: 0x0035E3F2
		public override void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			this.parent = parent;
			this.Spawn(ritual);
		}

		// Token: 0x06009424 RID: 37924 RVA: 0x00360204 File Offset: 0x0035E404
		protected override void Spawn(LordJob_Ritual ritual)
		{
			float num = 360f / (float)this.Props.numCopies;
			for (int i = 0; i < this.Props.numCopies; i++)
			{
				Vector3 a = Quaternion.AngleAxis(num * (float)i, Vector3.up) * Vector3.forward;
				Mote mote = this.SpawnMote(ritual, new Vector3?(ritual.selectedTarget.Cell.ToVector3Shifted() + a * this.Props.radius));
				if (mote != null)
				{
					this.parent.AddMoteToMaintain(mote);
					if (this.props.colorOverride != null)
					{
						mote.instanceColor = this.props.colorOverride.Value;
					}
					else
					{
						mote.instanceColor = this.parent.def.tintColor;
					}
				}
			}
			this.spawned = true;
		}
	}
}
