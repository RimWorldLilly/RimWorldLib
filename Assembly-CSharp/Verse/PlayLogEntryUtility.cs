using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001F5 RID: 501
	public static class PlayLogEntryUtility
	{
		// Token: 0x06000E52 RID: 3666 RVA: 0x0004A445 File Offset: 0x00048645
		public static IEnumerable<Rule> RulesForOptionalWeapon(string prefix, ThingDef weaponDef, ThingDef projectileDef)
		{
			PlayLogEntryUtility.<RulesForOptionalWeapon>d__0 <RulesForOptionalWeapon>d__ = new PlayLogEntryUtility.<RulesForOptionalWeapon>d__0(-2);
			<RulesForOptionalWeapon>d__.<>3__prefix = prefix;
			<RulesForOptionalWeapon>d__.<>3__weaponDef = weaponDef;
			<RulesForOptionalWeapon>d__.<>3__projectileDef = projectileDef;
			return <RulesForOptionalWeapon>d__;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0004A463 File Offset: 0x00048663
		public static IEnumerable<Rule> RulesForDamagedParts(string prefix, BodyDef body, List<BodyPartRecord> bodyParts, List<bool> bodyPartsDestroyed, Dictionary<string, string> constants)
		{
			PlayLogEntryUtility.<RulesForDamagedParts>d__1 <RulesForDamagedParts>d__ = new PlayLogEntryUtility.<RulesForDamagedParts>d__1(-2);
			<RulesForDamagedParts>d__.<>3__prefix = prefix;
			<RulesForDamagedParts>d__.<>3__bodyParts = bodyParts;
			<RulesForDamagedParts>d__.<>3__bodyPartsDestroyed = bodyPartsDestroyed;
			<RulesForDamagedParts>d__.<>3__constants = constants;
			return <RulesForDamagedParts>d__;
		}
	}
}
