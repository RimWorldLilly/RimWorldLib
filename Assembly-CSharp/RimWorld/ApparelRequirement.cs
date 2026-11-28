using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001592 RID: 5522
	public class ApparelRequirement : IExposable
	{
		// Token: 0x17001731 RID: 5937
		// (get) Token: 0x06008785 RID: 34693 RVA: 0x0030FAA1 File Offset: 0x0030DCA1
		public bool IsValid
		{
			get
			{
				if (this.isValid == null)
				{
					this.isValid = new bool?(this.AllRequiredApparel(Gender.None).Any<ThingDef>());
				}
				return this.isValid.Value;
			}
		}

		// Token: 0x06008786 RID: 34694 RVA: 0x0030FAD4 File Offset: 0x0030DCD4
		private bool MatchesBodyPartGroups(ThingDef apparel)
		{
			foreach (BodyPartGroupDef item in this.bodyPartGroupsMatchAny)
			{
				if (apparel.apparel.bodyPartGroups.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008787 RID: 34695 RVA: 0x0030FB3C File Offset: 0x0030DD3C
		private bool HasRequiredTag(ThingDef apparel)
		{
			if (this.requiredTags != null)
			{
				foreach (string item in apparel.apparel.tags)
				{
					if (this.requiredTags.Contains(item))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06008788 RID: 34696 RVA: 0x0030FBAC File Offset: 0x0030DDAC
		private static bool WearsAny(Pawn p, ThingDef apparel)
		{
			using (List<Apparel>.Enumerator enumerator = p.apparel.WornApparel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == apparel)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008789 RID: 34697 RVA: 0x0030FC0C File Offset: 0x0030DE0C
		public bool AllowedForPawn(Pawn p, ThingDef apparel, bool ignoreGender = false)
		{
			if (!apparel.IsApparel)
			{
				return false;
			}
			if (!apparel.apparel.PawnCanWear(p, ignoreGender))
			{
				return false;
			}
			if (apparel.apparel.tags == null)
			{
				return false;
			}
			bool flag = this.requiredDefs != null && this.requiredDefs.Contains(apparel);
			if (!flag)
			{
				foreach (string item in apparel.apparel.tags)
				{
					if ((this.requiredTags != null && this.requiredTags.Contains(item)) || (this.allowedTags != null && this.allowedTags.Contains(item)))
					{
						flag = true;
						break;
					}
				}
			}
			return flag && this.MatchesBodyPartGroups(apparel);
		}

		// Token: 0x0600878A RID: 34698 RVA: 0x0030FCE0 File Offset: 0x0030DEE0
		public IEnumerable<ThingDef> AllAllowedApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
		{
			ApparelRequirement.<AllAllowedApparelForPawn>d__12 <AllAllowedApparelForPawn>d__ = new ApparelRequirement.<AllAllowedApparelForPawn>d__12(-2);
			<AllAllowedApparelForPawn>d__.<>4__this = this;
			<AllAllowedApparelForPawn>d__.<>3__p = p;
			<AllAllowedApparelForPawn>d__.<>3__ignoreGender = ignoreGender;
			<AllAllowedApparelForPawn>d__.<>3__includeWorn = includeWorn;
			return <AllAllowedApparelForPawn>d__;
		}

		// Token: 0x0600878B RID: 34699 RVA: 0x0030FD08 File Offset: 0x0030DF08
		public bool RequiredForPawn(Pawn p, ThingDef apparel, bool ignoreGender = false)
		{
			return apparel.IsApparel && apparel.apparel.PawnCanWear(p, ignoreGender) && apparel.apparel.tags != null && ((this.requiredDefs != null && this.requiredDefs.Contains(apparel)) || this.HasRequiredTag(apparel)) && this.MatchesBodyPartGroups(apparel);
		}

		// Token: 0x0600878C RID: 34700 RVA: 0x0030FD6A File Offset: 0x0030DF6A
		public IEnumerable<ThingDef> AllRequiredApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
		{
			ApparelRequirement.<AllRequiredApparelForPawn>d__14 <AllRequiredApparelForPawn>d__ = new ApparelRequirement.<AllRequiredApparelForPawn>d__14(-2);
			<AllRequiredApparelForPawn>d__.<>4__this = this;
			<AllRequiredApparelForPawn>d__.<>3__p = p;
			<AllRequiredApparelForPawn>d__.<>3__ignoreGender = ignoreGender;
			<AllRequiredApparelForPawn>d__.<>3__includeWorn = includeWorn;
			return <AllRequiredApparelForPawn>d__;
		}

		// Token: 0x0600878D RID: 34701 RVA: 0x0030FD8F File Offset: 0x0030DF8F
		public IEnumerable<ThingDef> AllRequiredApparel(Gender gender = Gender.None)
		{
			ApparelRequirement.<AllRequiredApparel>d__15 <AllRequiredApparel>d__ = new ApparelRequirement.<AllRequiredApparel>d__15(-2);
			<AllRequiredApparel>d__.<>4__this = this;
			<AllRequiredApparel>d__.<>3__gender = gender;
			return <AllRequiredApparel>d__;
		}

		// Token: 0x0600878E RID: 34702 RVA: 0x0030FDA8 File Offset: 0x0030DFA8
		public bool ApparelMeetsRequirement(ThingDef thingDef, bool allowUnmatched = true)
		{
			bool flag = false;
			for (int i = 0; i < this.bodyPartGroupsMatchAny.Count; i++)
			{
				if (thingDef.apparel.bodyPartGroups.Contains(this.bodyPartGroupsMatchAny[i]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return allowUnmatched;
			}
			if (this.requiredDefs != null && this.requiredDefs.Contains(thingDef))
			{
				return true;
			}
			if (this.requiredTags != null)
			{
				for (int j = 0; j < this.requiredTags.Count; j++)
				{
					if (thingDef.apparel.tags.Contains(this.requiredTags[j]))
					{
						return true;
					}
				}
			}
			if (this.allowedTags != null)
			{
				for (int k = 0; k < this.allowedTags.Count; k++)
				{
					if (thingDef.apparel.tags.Contains(this.allowedTags[k]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600878F RID: 34703 RVA: 0x0030FE8C File Offset: 0x0030E08C
		public bool IsMet(Pawn p)
		{
			foreach (Apparel apparel in p.apparel.WornApparel)
			{
				bool flag = false;
				for (int i = 0; i < this.bodyPartGroupsMatchAny.Count; i++)
				{
					if (apparel.def.apparel.bodyPartGroups.Contains(this.bodyPartGroupsMatchAny[i]))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (this.requiredDefs != null && this.requiredDefs.Contains(apparel.def))
					{
						return true;
					}
					if (this.requiredTags != null)
					{
						for (int j = 0; j < this.requiredTags.Count; j++)
						{
							if (apparel.def.apparel.tags.Contains(this.requiredTags[j]))
							{
								return true;
							}
						}
					}
					if (this.allowedTags != null)
					{
						for (int k = 0; k < this.allowedTags.Count; k++)
						{
							if (apparel.def.apparel.tags.Contains(this.allowedTags[k]))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06008790 RID: 34704 RVA: 0x0030FFF4 File Offset: 0x0030E1F4
		public bool SameApparelAs(ApparelRequirement other)
		{
			return this.requiredDefs.SetsEqual(other.requiredDefs) && this.requiredTags.SetsEqual(other.requiredTags) && this.allowedTags.SetsEqual(other.allowedTags);
		}

		// Token: 0x06008791 RID: 34705 RVA: 0x0031002F File Offset: 0x0030E22F
		public bool IsActive(Pawn forPawn)
		{
			return forPawn.apparel != null && forPawn.apparel.ActiveRequirementsForReading.Contains(this);
		}

		// Token: 0x06008792 RID: 34706 RVA: 0x0031004C File Offset: 0x0030E24C
		public ThingDef RandomRequiredApparelForPawnInGeneration(Pawn p, Func<ThingDef, bool> validator)
		{
			ThingDef result = null;
			Predicate<BodyPartGroupDef> <>9__2;
			Predicate<string> <>9__3;
			if (!DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef a)
			{
				if (a.IsApparel && a.apparel.tags != null)
				{
					List<BodyPartGroupDef> bodyPartGroups = a.apparel.bodyPartGroups;
					Predicate<BodyPartGroupDef> predicate;
					if ((predicate = <>9__2) == null)
					{
						predicate = (<>9__2 = ((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b)));
					}
					if (bodyPartGroups.Any(predicate))
					{
						List<string> tags = a.apparel.tags;
						Predicate<string> predicate2;
						if ((predicate2 = <>9__3) == null)
						{
							predicate2 = (<>9__3 = ((string t) => this.requiredTags.Contains(t)));
						}
						if (tags.Any(predicate2) && a.apparel.PawnCanWear(p, false))
						{
							return validator == null || validator(a);
						}
					}
				}
				return false;
			}).TryRandomElementByWeight((ThingDef a) => a.generateCommonality, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06008793 RID: 34707 RVA: 0x003100B8 File Offset: 0x0030E2B8
		public override string ToString()
		{
			string text = "";
			if (this.requiredTags != null)
			{
				text += string.Join(",", this.requiredTags.ToArray());
			}
			if (this.allowedTags != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "|";
				}
				text += string.Join(",", this.allowedTags.ToArray());
			}
			if (this.requiredDefs != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "|";
				}
				text += string.Join<TaggedString>(",", (from d in this.requiredDefs
				select d.LabelCap).ToArray<TaggedString>());
			}
			return "(" + string.Join(",", (from a in this.bodyPartGroupsMatchAny
			select a.defName).ToArray<string>()) + ") -> " + text;
		}

		// Token: 0x06008794 RID: 34708 RVA: 0x003101CC File Offset: 0x0030E3CC
		public void ExposeData()
		{
			Scribe_Collections.Look<BodyPartGroupDef>(ref this.bodyPartGroupsMatchAny, "bodyPartGroupsMatchAny", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<ThingDef>(ref this.requiredDefs, "requiredDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.requiredTags, "requiredTags", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.allowedTags, "allowedTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.groupLabel, "groupLabel", null, false);
		}

		// Token: 0x06008795 RID: 34709 RVA: 0x00310244 File Offset: 0x0030E444
		public virtual void CopyTo(ApparelRequirement other)
		{
			if (this.bodyPartGroupsMatchAny != null)
			{
				if (other.bodyPartGroupsMatchAny == null)
				{
					other.bodyPartGroupsMatchAny = new List<BodyPartGroupDef>();
				}
				other.bodyPartGroupsMatchAny.Clear();
				other.bodyPartGroupsMatchAny.AddRange(this.bodyPartGroupsMatchAny);
			}
			else
			{
				other.bodyPartGroupsMatchAny = null;
			}
			if (this.requiredTags != null)
			{
				if (other.requiredTags == null)
				{
					other.requiredTags = new List<string>();
				}
				other.requiredTags.Clear();
				other.requiredTags.AddRange(this.requiredTags);
			}
			else
			{
				other.requiredTags = null;
			}
			if (this.allowedTags != null)
			{
				if (other.allowedTags == null)
				{
					other.allowedTags = new List<string>();
				}
				other.allowedTags.Clear();
				other.allowedTags.AddRange(this.allowedTags);
			}
			else
			{
				other.allowedTags = null;
			}
			if (this.requiredDefs != null)
			{
				if (other.requiredDefs == null)
				{
					other.requiredDefs = new List<ThingDef>();
				}
				other.requiredDefs.Clear();
				other.requiredDefs.AddRange(this.requiredDefs);
			}
			else
			{
				other.requiredDefs = null;
			}
			other.groupLabel = this.groupLabel;
		}

		// Token: 0x040053B5 RID: 21429
		public List<BodyPartGroupDef> bodyPartGroupsMatchAny;

		// Token: 0x040053B6 RID: 21430
		public List<string> requiredTags;

		// Token: 0x040053B7 RID: 21431
		public List<string> allowedTags;

		// Token: 0x040053B8 RID: 21432
		public List<ThingDef> requiredDefs;

		// Token: 0x040053B9 RID: 21433
		[MustTranslate]
		public string groupLabel;

		// Token: 0x040053BA RID: 21434
		[Unsaved(false)]
		private bool? isValid;
	}
}
