using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBC RID: 3772
	[StaticConstructorOnStartup]
	public class CompNeuralSupercharger : CompRechargeable
	{
		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x06006344 RID: 25412 RVA: 0x0023ADDF File Offset: 0x00238FDF
		private static Texture2D ColonistOnlyCommandTex
		{
			get
			{
				if (CompNeuralSupercharger.colonistOnlyCommandTex == null)
				{
					CompNeuralSupercharger.colonistOnlyCommandTex = ContentFinder<Texture2D>.Get("UI/Gizmos/NeuralSupercharger_AllowGuests", true);
				}
				return CompNeuralSupercharger.colonistOnlyCommandTex;
			}
		}

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x06006345 RID: 25413 RVA: 0x0023AE03 File Offset: 0x00239003
		private CompProperties_NeuralSupercharger Props
		{
			get
			{
				return (CompProperties_NeuralSupercharger)this.props;
			}
		}

		// Token: 0x06006346 RID: 25414 RVA: 0x0023AE10 File Offset: 0x00239010
		public bool CanAutoUse(Pawn pawn)
		{
			if (!this.allowGuests && pawn.IsQuestLodger())
			{
				return false;
			}
			switch (this.autoUseMode)
			{
			case CompNeuralSupercharger.AutoUseMode.NoAutoUse:
				return false;
			case CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire:
				return pawn.Ideo != null && pawn.Ideo.HasPrecept(PreceptDefOf.NeuralSupercharge_Preferred);
			case CompNeuralSupercharger.AutoUseMode.AutoUseForEveryone:
				return true;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unknown auto use mode: ");
				defaultInterpolatedStringHandler.AppendFormatted<CompNeuralSupercharger.AutoUseMode>(this.autoUseMode);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return false;
			}
			}
		}

		// Token: 0x06006347 RID: 25415 RVA: 0x0023AE99 File Offset: 0x00239099
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.autoUseMode = CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire;
		}

		// Token: 0x06006348 RID: 25416 RVA: 0x0023AEA9 File Offset: 0x002390A9
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<CompNeuralSupercharger.AutoUseMode>(ref this.autoUseMode, "autoUseMode", CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire, false);
			Scribe_Values.Look<bool>(ref this.allowGuests, "allowGuests", false, false);
		}

		// Token: 0x06006349 RID: 25417 RVA: 0x0023AED8 File Offset: 0x002390D8
		public override void CompTick()
		{
			base.CompTick();
			if (this.Props.effectCharged != null)
			{
				if (this.parent.Spawned)
				{
					if (base.Charged)
					{
						if (this.effecterCharged == null)
						{
							this.effecterCharged = this.Props.effectCharged.Spawn();
							this.effecterCharged.Trigger(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false), -1);
						}
						this.effecterCharged.EffectTick(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false));
						return;
					}
					if (this.effecterCharged != null)
					{
						this.DespawnEffecter();
						return;
					}
				}
				else
				{
					this.DespawnEffecter();
				}
			}
		}

		// Token: 0x0600634A RID: 25418 RVA: 0x0023AFB0 File Offset: 0x002391B0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("Neural supercharger"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x0600634B RID: 25419 RVA: 0x0023AFD2 File Offset: 0x002391D2
		private void DespawnEffecter()
		{
			Effecter effecter = this.effecterCharged;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecterCharged = null;
		}

		// Token: 0x0600634C RID: 25420 RVA: 0x0023AFEC File Offset: 0x002391EC
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompNeuralSupercharger.<CompFloatMenuOptions>d__15 <CompFloatMenuOptions>d__ = new CompNeuralSupercharger.<CompFloatMenuOptions>d__15(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x0600634D RID: 25421 RVA: 0x0023B003 File Offset: 0x00239203
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompNeuralSupercharger.<CompGetGizmosExtra>d__16 <CompGetGizmosExtra>d__ = new CompNeuralSupercharger.<CompGetGizmosExtra>d__16(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0400377A RID: 14202
		private static Texture2D colonistOnlyCommandTex;

		// Token: 0x0400377B RID: 14203
		public CompNeuralSupercharger.AutoUseMode autoUseMode = CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire;

		// Token: 0x0400377C RID: 14204
		public bool allowGuests;

		// Token: 0x0400377D RID: 14205
		private Effecter effecterCharged;

		// Token: 0x02002F17 RID: 12055
		public enum AutoUseMode
		{
			// Token: 0x0400BF15 RID: 48917
			NoAutoUse,
			// Token: 0x0400BF16 RID: 48918
			AutoUseWithDesire,
			// Token: 0x0400BF17 RID: 48919
			AutoUseForEveryone
		}
	}
}
