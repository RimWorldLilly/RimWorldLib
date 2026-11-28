using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001892 RID: 6290
	public class CompFloorEtching : CompFloorEtchingRambling
	{
		// Token: 0x06009D93 RID: 40339 RVA: 0x0038A0C0 File Offset: 0x003882C0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			LabyrinthMapComponent component = this.parent.Map.GetComponent<LabyrinthMapComponent>();
			if (component == null)
			{
				return;
			}
			float angleFlat = (component.labyrinthObelisk.Position - this.parent.Position).AngleFlat;
			if (angleFlat >= 315f || angleFlat < 45f)
			{
				this.direction = Rot4.North;
				return;
			}
			if (angleFlat >= 45f && angleFlat < 135f)
			{
				this.direction = Rot4.East;
				return;
			}
			if (angleFlat >= 135f && angleFlat < 225f)
			{
				this.direction = Rot4.South;
				return;
			}
			if (angleFlat >= 225f && angleFlat < 315f)
			{
				this.direction = Rot4.West;
			}
		}

		// Token: 0x06009D94 RID: 40340 RVA: 0x0038A180 File Offset: 0x00388380
		protected override void OnInteracted(Pawn caster)
		{
			this.deciphered = true;
			base.GenerateMessage();
			TaggedString label = "LetterLabelFloorEtchings".Translate();
			TaggedString text = "LetterFloorEtchings".Translate(caster.Named("PAWN"), this.message.Named("RAMBLINGS"), this.direction.ToStringHuman().ToLower().Named("DIRECTION"));
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, this.parent, null, null, null, null, 0, true);
		}

		// Token: 0x06009D95 RID: 40341 RVA: 0x0038A20C File Offset: 0x0038840C
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.deciphered)
			{
				stringBuilder.AppendLineIfNotEmpty();
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("FloorEtchingsInspectorExit".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(this.direction.ToStringHuman());
				stringBuilder2.Append(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			else
			{
				stringBuilder.Append(base.CompInspectStringExtra());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06009D96 RID: 40342 RVA: 0x0038A28C File Offset: 0x0038848C
		public override string CompTipStringExtra()
		{
			if (!this.deciphered)
			{
				return "";
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("FloorEtchingsInspectorExit".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(this.direction.ToStringHuman());
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06009D97 RID: 40343 RVA: 0x0038A2E8 File Offset: 0x003884E8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Rot4>(ref this.direction, "direction", default(Rot4), false);
		}

		// Token: 0x04005D56 RID: 23894
		private Rot4 direction;
	}
}
