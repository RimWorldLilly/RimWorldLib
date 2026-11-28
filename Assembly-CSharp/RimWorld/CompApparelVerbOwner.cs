using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001811 RID: 6161
	public class CompApparelVerbOwner : ThingComp, IVerbOwner
	{
		// Token: 0x17001A2B RID: 6699
		// (get) Token: 0x060099E5 RID: 39397 RVA: 0x00379FA8 File Offset: 0x003781A8
		public CompProperties_ApparelVerbOwner Props
		{
			get
			{
				return this.props as CompProperties_ApparelVerbOwner;
			}
		}

		// Token: 0x17001A2C RID: 6700
		// (get) Token: 0x060099E6 RID: 39398 RVA: 0x00379FB5 File Offset: 0x003781B5
		public Pawn Wearer
		{
			get
			{
				Pawn_ApparelTracker pawn_ApparelTracker = base.ParentHolder as Pawn_ApparelTracker;
				if (pawn_ApparelTracker == null)
				{
					return null;
				}
				return pawn_ApparelTracker.pawn;
			}
		}

		// Token: 0x17001A2D RID: 6701
		// (get) Token: 0x060099E7 RID: 39399 RVA: 0x00379FCD File Offset: 0x003781CD
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x17001A2E RID: 6702
		// (get) Token: 0x060099E8 RID: 39400 RVA: 0x00379FDF File Offset: 0x003781DF
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x17001A2F RID: 6703
		// (get) Token: 0x060099E9 RID: 39401 RVA: 0x00379FF1 File Offset: 0x003781F1
		public ImplementOwnerTypeDef ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x17001A30 RID: 6704
		// (get) Token: 0x060099EA RID: 39402 RVA: 0x00379FF8 File Offset: 0x003781F8
		public Thing ConstantCaster
		{
			get
			{
				return this.Wearer;
			}
		}

		// Token: 0x060099EB RID: 39403 RVA: 0x0037A000 File Offset: 0x00378200
		public string UniqueVerbOwnerID()
		{
			return "CompVerbOwner_" + this.parent.ThingID;
		}

		// Token: 0x17001A31 RID: 6705
		// (get) Token: 0x060099EC RID: 39404 RVA: 0x0037A017 File Offset: 0x00378217
		public virtual string GizmoExtraLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060099ED RID: 39405 RVA: 0x0037A01A File Offset: 0x0037821A
		public bool VerbsStillUsableBy(Pawn p)
		{
			return this.Wearer == p;
		}

		// Token: 0x17001A32 RID: 6706
		// (get) Token: 0x060099EE RID: 39406 RVA: 0x0037A025 File Offset: 0x00378225
		public VerbTracker VerbTracker
		{
			get
			{
				if (this.verbTracker == null)
				{
					this.verbTracker = new VerbTracker(this);
				}
				return this.verbTracker;
			}
		}

		// Token: 0x17001A33 RID: 6707
		// (get) Token: 0x060099EF RID: 39407 RVA: 0x0037A041 File Offset: 0x00378241
		public List<Verb> AllVerbs
		{
			get
			{
				return this.VerbTracker.AllVerbs;
			}
		}

		// Token: 0x060099F0 RID: 39408 RVA: 0x0037A04E File Offset: 0x0037824E
		public override void PostExposeData()
		{
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.SetupVerbs();
			}
		}

		// Token: 0x060099F1 RID: 39409 RVA: 0x0037A078 File Offset: 0x00378278
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.SetupVerbs();
		}

		// Token: 0x060099F2 RID: 39410 RVA: 0x0037A088 File Offset: 0x00378288
		private void SetupVerbs()
		{
			for (int i = 0; i < this.AllVerbs.Count; i++)
			{
				this.AllVerbs[i].caster = this.Wearer;
			}
		}

		// Token: 0x060099F3 RID: 39411 RVA: 0x0037A0C2 File Offset: 0x003782C2
		public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
		{
			CompApparelVerbOwner.<CompGetWornGizmosExtra>d__24 <CompGetWornGizmosExtra>d__ = new CompApparelVerbOwner.<CompGetWornGizmosExtra>d__24(-2);
			<CompGetWornGizmosExtra>d__.<>4__this = this;
			return <CompGetWornGizmosExtra>d__;
		}

		// Token: 0x060099F4 RID: 39412 RVA: 0x0037A0D4 File Offset: 0x003782D4
		private Command_VerbTarget CreateVerbTargetCommand(Thing gear, Verb verb)
		{
			Command_VerbOwner command_VerbOwner = new Command_VerbOwner(this);
			command_VerbOwner.defaultDesc = gear.def.description;
			command_VerbOwner.hotKey = this.Props.hotKey;
			command_VerbOwner.defaultLabel = verb.verbProps.label;
			command_VerbOwner.verb = verb;
			if (verb.verbProps.defaultProjectile != null && verb.verbProps.commandIcon == null)
			{
				command_VerbOwner.icon = verb.verbProps.defaultProjectile.uiIcon;
				command_VerbOwner.iconAngle = verb.verbProps.defaultProjectile.uiIconAngle;
				command_VerbOwner.iconOffset = verb.verbProps.defaultProjectile.uiIconOffset;
				command_VerbOwner.overrideColor = new Color?(verb.verbProps.defaultProjectile.graphicData.color);
			}
			else
			{
				command_VerbOwner.icon = ((verb.UIIcon != BaseContent.BadTex) ? verb.UIIcon : gear.def.uiIcon);
				command_VerbOwner.iconAngle = gear.def.uiIconAngle;
				command_VerbOwner.iconOffset = gear.def.uiIconOffset;
				command_VerbOwner.defaultIconColor = gear.DrawColor;
			}
			string reason;
			if (!this.Wearer.IsColonistPlayerControlled)
			{
				command_VerbOwner.Disable("CannotOrderNonControlled".Translate());
			}
			else if (verb.verbProps.violent && this.Wearer.WorkTagIsDisabled(WorkTags.Violent))
			{
				command_VerbOwner.Disable("IsIncapableOfViolenceLower".Translate(this.Wearer.LabelShort, this.Wearer).CapitalizeFirst() + ".");
			}
			else if (!this.CanBeUsed(out reason))
			{
				command_VerbOwner.Disable(reason);
			}
			return command_VerbOwner;
		}

		// Token: 0x060099F5 RID: 39413 RVA: 0x0037A28E File Offset: 0x0037848E
		public virtual void UsedOnce()
		{
		}

		// Token: 0x060099F6 RID: 39414 RVA: 0x0037A290 File Offset: 0x00378490
		public virtual bool CanBeUsed(out string reason)
		{
			reason = "";
			if (this.parent.MapHeld == null)
			{
				return false;
			}
			if (this.parent.MapHeld.IsPocketMap)
			{
				if (this.VerbProperties.Any((VerbProperties vp) => !vp.useableInPocketMaps))
				{
					reason = "CannotUseReason_PocketMap".Translate(this.parent.MapHeld.generatorDef.label);
					return false;
				}
			}
			if (this.parent.MapHeld.Biome.inVacuum)
			{
				if (this.VerbProperties.Any((VerbProperties vp) => !vp.useableInVacuum))
				{
					reason = "CannotFunctionInVacuum".Translate();
					return false;
				}
			}
			return true;
		}

		// Token: 0x04005BBA RID: 23482
		private VerbTracker verbTracker;
	}
}
