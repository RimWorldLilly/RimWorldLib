using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x02000736 RID: 1846
	public class VerbTracker : IExposable
	{
		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06003A87 RID: 14983 RVA: 0x001754C6 File Offset: 0x001736C6
		public List<Verb> AllVerbs
		{
			get
			{
				if (this.verbs == null)
				{
					this.InitVerbsFromZero();
				}
				return this.verbs;
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06003A88 RID: 14984 RVA: 0x001754DC File Offset: 0x001736DC
		public Verb PrimaryVerb
		{
			get
			{
				if (this.verbs == null)
				{
					this.InitVerbsFromZero();
				}
				for (int i = 0; i < this.verbs.Count; i++)
				{
					if (this.verbs[i].verbProps.isPrimary)
					{
						return this.verbs[i];
					}
				}
				return null;
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06003A89 RID: 14985 RVA: 0x00175534 File Offset: 0x00173734
		public bool AnyVerbBursting
		{
			get
			{
				for (int i = 0; i < this.verbs.Count; i++)
				{
					if (this.verbs[i].state == VerbState.Bursting)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x0017556E File Offset: 0x0017376E
		public VerbTracker(IVerbOwner directOwner)
		{
			this.directOwner = directOwner;
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x00175580 File Offset: 0x00173780
		public void VerbsTick()
		{
			if (this.verbs == null)
			{
				return;
			}
			for (int i = 0; i < this.verbs.Count; i++)
			{
				this.verbs[i].VerbTick();
			}
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x001755BD File Offset: 0x001737BD
		public IEnumerable<Command> GetVerbsCommands()
		{
			VerbTracker.<GetVerbsCommands>d__10 <GetVerbsCommands>d__ = new VerbTracker.<GetVerbsCommands>d__10(-2);
			<GetVerbsCommands>d__.<>4__this = this;
			return <GetVerbsCommands>d__;
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x001755D0 File Offset: 0x001737D0
		private Command_VerbTarget CreateVerbTargetCommand(Thing ownerThing, Verb verb)
		{
			Command_VerbTarget command_VerbTarget = new Command_VerbTarget();
			command_VerbTarget.defaultDesc = ownerThing.LabelCap + ": " + ownerThing.def.description.CapitalizeFirst();
			command_VerbTarget.ownerThing = ownerThing;
			command_VerbTarget.tutorTag = "VerbTarget";
			command_VerbTarget.verb = verb;
			if (verb.caster.Faction != Faction.OfPlayer && !DebugSettings.ShowDevGizmos)
			{
				command_VerbTarget.Disable("CannotOrderNonControlled".Translate());
			}
			else if (verb.CasterIsPawn)
			{
				string reason;
				if (verb.CasterPawn.RaceProps.IsMechanoid && !MechanitorUtility.EverControllable(verb.CasterPawn) && !DebugSettings.ShowDevGizmos)
				{
					command_VerbTarget.Disable("CannotOrderNonControlled".Translate());
				}
				else if (verb.CasterPawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					command_VerbTarget.Disable("IsIncapableOfViolence".Translate(verb.CasterPawn.LabelShort, verb.CasterPawn));
				}
				else if (!verb.CasterPawn.Drafted && !DebugSettings.ShowDevGizmos)
				{
					command_VerbTarget.Disable("IsNotDrafted".Translate(verb.CasterPawn.LabelShort, verb.CasterPawn));
				}
				else if (verb is Verb_LaunchProjectile)
				{
					Apparel apparel = verb.FirstApparelPreventingShooting();
					if (apparel != null)
					{
						command_VerbTarget.Disable("ApparelPreventsShooting".Translate(verb.CasterPawn.Named("PAWN"), apparel.Named("APPAREL")).CapitalizeFirst());
					}
				}
				else if (EquipmentUtility.RolePreventsFromUsing(verb.CasterPawn, verb.EquipmentSource, out reason))
				{
					command_VerbTarget.Disable(reason);
				}
			}
			CompUniqueWeapon compUniqueWeapon;
			if ((!verb.EquipmentSource.TryGetComp(out compUniqueWeapon) || !compUniqueWeapon.IgnoreAccuracyMaluses) && verb.caster.Spawned && verb.caster.Map.weatherManager.CurWeatherMaxRangeCap >= 0f)
			{
				command_VerbTarget.defaultDescPostfix = "\n\n" + ("WeatherMaxRangeCap".Translate() + ": " + verb.caster.Map.weatherManager.curWeather.LabelCap).Colorize(ColoredText.WarningColor);
			}
			return command_VerbTarget;
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x00175824 File Offset: 0x00173A24
		public Verb GetVerb(VerbCategory category)
		{
			List<Verb> allVerbs = this.AllVerbs;
			if (allVerbs != null)
			{
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (allVerbs[i].verbProps.category == category)
					{
						return allVerbs[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x0017586C File Offset: 0x00173A6C
		public void ExposeData()
		{
			Scribe_Collections.Look<Verb>(ref this.verbs, "verbs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.verbs != null)
			{
				if (this.verbs.RemoveAll((Verb x) => x == null) != 0)
				{
					Log.Error("Some verbs were null after loading. directOwner=" + this.directOwner.ToStringSafe<IVerbOwner>());
				}
				List<Verb> sources = this.verbs;
				this.verbs = new List<Verb>();
				this.InitVerbs(delegate(Type type, string id)
				{
					Verb verb = sources.FirstOrDefault((Verb v) => v.loadID == id && v.GetType() == type);
					if (verb == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(79, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Replaced verb ");
						defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
						defaultInterpolatedStringHandler.AppendLiteral("/");
						defaultInterpolatedStringHandler.AppendFormatted(id);
						defaultInterpolatedStringHandler.AppendLiteral("; may have been changed through a version update or a mod change");
						Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
						verb = (Verb)Activator.CreateInstance(type);
					}
					this.verbs.Add(verb);
					return verb;
				});
			}
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x0017591B File Offset: 0x00173B1B
		public void InitVerbsFromZero()
		{
			this.verbs = new List<Verb>();
			this.InitVerbs(delegate(Type type, string id)
			{
				Verb verb = (Verb)Activator.CreateInstance(type);
				this.verbs.Add(verb);
				return verb;
			});
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x0017593C File Offset: 0x00173B3C
		private void InitVerbs(Func<Type, string, Verb> creator)
		{
			List<VerbProperties> verbProperties = this.directOwner.VerbProperties;
			if (verbProperties != null)
			{
				for (int i = 0; i < verbProperties.Count; i++)
				{
					try
					{
						VerbProperties verbProperties2 = verbProperties[i];
						string text = Verb.CalculateUniqueLoadID(this.directOwner, i);
						this.InitVerb(creator(verbProperties2.verbClass, text), verbProperties2, null, null, text);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate Verb (directOwner=";
						string str2 = this.directOwner.ToStringSafe<IVerbOwner>();
						string str3 = "): ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			List<Tool> tools = this.directOwner.Tools;
			if (tools != null)
			{
				for (int j = 0; j < tools.Count; j++)
				{
					Tool tool = tools[j];
					foreach (ManeuverDef maneuverDef in tool.Maneuvers)
					{
						try
						{
							VerbProperties verb = maneuverDef.verb;
							string text2 = Verb.CalculateUniqueLoadID(this.directOwner, tool, maneuverDef);
							this.InitVerb(creator(verb.verbClass, text2), verb, tool, maneuverDef, text2);
						}
						catch (Exception ex3)
						{
							string str4 = "Could not instantiate Verb (directOwner=";
							string str5 = this.directOwner.ToStringSafe<IVerbOwner>();
							string str6 = "): ";
							Exception ex4 = ex3;
							Log.Error(str4 + str5 + str6 + ((ex4 != null) ? ex4.ToString() : null));
						}
					}
				}
			}
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x00175AC8 File Offset: 0x00173CC8
		private void InitVerb(Verb verb, VerbProperties properties, Tool tool, ManeuverDef maneuver, string id)
		{
			verb.loadID = id;
			verb.verbProps = properties;
			verb.verbTracker = this;
			verb.tool = tool;
			verb.maneuver = maneuver;
			verb.caster = this.directOwner.ConstantCaster;
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x00175B00 File Offset: 0x00173D00
		public void VerbsNeedReinitOnLoad()
		{
			this.verbs = null;
		}

		// Token: 0x04002697 RID: 9879
		public IVerbOwner directOwner;

		// Token: 0x04002698 RID: 9880
		private List<Verb> verbs;
	}
}
