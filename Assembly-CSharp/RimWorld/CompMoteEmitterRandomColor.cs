using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001913 RID: 6419
	public class CompMoteEmitterRandomColor : CompMoteEmitter
	{
		// Token: 0x17001BBE RID: 7102
		// (get) Token: 0x0600A0DF RID: 41183 RVA: 0x00398532 File Offset: 0x00396732
		public CompProperties_MoteEmitterRandomColor Props
		{
			get
			{
				return (CompProperties_MoteEmitterRandomColor)this.props;
			}
		}

		// Token: 0x0600A0E0 RID: 41184 RVA: 0x0039853F File Offset: 0x0039673F
		public override void Emit()
		{
			base.Emit();
			this.mote.instanceColor = this.Props.colors.RandomElement<Color>();
		}
	}
}
