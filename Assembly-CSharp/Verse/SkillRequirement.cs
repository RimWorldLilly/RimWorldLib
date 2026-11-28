using System;
using System.Runtime.CompilerServices;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020001AA RID: 426
	public class SkillRequirement
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x0003E68C File Offset: 0x0003C88C
		public string Summary
		{
			get
			{
				if (this.skill == null)
				{
					return "";
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(this.skill.LabelCap);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.minLevel);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0003E6F0 File Offset: 0x0003C8F0
		public bool PawnSatisfies(Pawn pawn)
		{
			return (pawn.IsColonyMechPlayerControlled && pawn.RaceProps.mechFixedSkillLevel >= this.minLevel && pawn.RaceProps.mechEnabledWorkTypes.Any((WorkTypeDef w) => w.relevantSkills.NotNullAndContains(this.skill))) || (pawn.skills != null && pawn.skills.GetSkill(this.skill).Level >= this.minLevel);
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0003E763 File Offset: 0x0003C963
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null, null);
			this.minLevel = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0003E78F File Offset: 0x0003C98F
		public override string ToString()
		{
			if (this.skill == null)
			{
				return "null-skill-requirement";
			}
			return this.skill.defName + "-" + this.minLevel.ToString();
		}

		// Token: 0x04000AEC RID: 2796
		public SkillDef skill;

		// Token: 0x04000AED RID: 2797
		public int minLevel;
	}
}
