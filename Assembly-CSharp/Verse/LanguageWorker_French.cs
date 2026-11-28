using System;
using System.Text.RegularExpressions;

namespace Verse
{
	// Token: 0x0200021B RID: 539
	public class LanguageWorker_French : LanguageWorker
	{
		// Token: 0x06000FAA RID: 4010 RVA: 0x00058635 File Offset: 0x00056835
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return "des " + str;
			}
			return ((gender == Gender.Female) ? "une " : "un ") + str;
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00058664 File Offset: 0x00056864
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return "les " + str;
			}
			char ch = str[0];
			if (this.IsVowel(ch))
			{
				return "l'" + str;
			}
			return ((gender == Gender.Female) ? "la " : "le ") + str;
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x000586C3 File Offset: 0x000568C3
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			if (number != 1)
			{
				return number.ToString() + "e";
			}
			return number.ToString() + "er";
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x000586EC File Offset: 0x000568EC
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			char c = str[str.Length - 1];
			if (c == 's' || c == 'x' || c == 'z')
			{
				return str;
			}
			uint num = <PrivateImplementationDetails>.ComputeStringHash(str);
			if (num <= 1926548781U)
			{
				if (num <= 220114272U)
				{
					if (num != 131056644U)
					{
						if (num != 220114272U)
						{
							goto IL_19C;
						}
						if (!(str == "caillou"))
						{
							goto IL_19C;
						}
						goto IL_190;
					}
					else if (!(str == "lieu"))
					{
						goto IL_19C;
					}
				}
				else if (num != 362463927U)
				{
					if (num != 1052243577U)
					{
						if (num != 1926548781U)
						{
							goto IL_19C;
						}
						if (!(str == "bleu"))
						{
							goto IL_19C;
						}
					}
					else if (!(str == "fatal"))
					{
						goto IL_19C;
					}
				}
				else if (!(str == "final"))
				{
					goto IL_19C;
				}
			}
			else if (num <= 2593919137U)
			{
				if (num != 2378530198U)
				{
					if (num != 2593919137U)
					{
						goto IL_19C;
					}
					if (!(str == "genou"))
					{
						goto IL_19C;
					}
					goto IL_190;
				}
				else
				{
					if (!(str == "travail"))
					{
						goto IL_19C;
					}
					return str.Substring(0, str.Length - 3) + "aux";
				}
			}
			else if (num != 2731127275U)
			{
				if (num != 2797929741U)
				{
					if (num != 3614771774U)
					{
						goto IL_19C;
					}
					if (!(str == "bijou"))
					{
						goto IL_19C;
					}
					goto IL_190;
				}
				else if (!(str == "émeu"))
				{
					goto IL_19C;
				}
			}
			else if (!(str == "banal"))
			{
				goto IL_19C;
			}
			return str + "s";
			IL_190:
			return str + "x";
			IL_19C:
			if (str.EndsWith("al"))
			{
				return str.Substring(0, str.Length - 2) + "aux";
			}
			if (str.EndsWith("au") | str.EndsWith("eu"))
			{
				return str + "x";
			}
			return str + "s";
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x000588EC File Offset: 0x00056AEC
		public override string PostProcessed(string str)
		{
			return this.PostProcessedInt(base.PostProcessed(str));
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x000588FB File Offset: 0x00056AFB
		public bool IsVowel(char ch)
		{
			return "aàâäæeéèêëiîïoôöœuùüûhAÀÂÄÆEÉÈÊËIÎÏOÔÖŒUÙÜÛH".IndexOf(ch) >= 0;
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00058910 File Offset: 0x00056B10
		private string PostProcessedInt(string str)
		{
			str = this.ElisionE.Replace(str, "$1'$2");
			str = this.ElisionLa.Replace(str, "$1'$2");
			str = this.ElisionSi.Replace(str, "$1'$2");
			str = this.DeLe.Replace(str, "$1$2");
			str = this.ALe.Replace(str, new MatchEvaluator(this.ReplaceALe));
			return str;
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00058984 File Offset: 0x00056B84
		private string ReplaceALe(Match match)
		{
			string a = match.ToString();
			if (a == "à le")
			{
				return "au";
			}
			if (a == "à les")
			{
				return "aux";
			}
			if (a == "À le")
			{
				return "Au";
			}
			if (!(a == "À les"))
			{
				return match.ToString();
			}
			return "Aux";
		}

		// Token: 0x04000EDC RID: 3804
		private Regex ElisionE = new Regex("\\b([cdjlmnst]|qu|quoiqu|lorsqu)e ([aàâäeéèêëiîïoôöuùüûh])", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x04000EDD RID: 3805
		private Regex ElisionLa = new Regex("\\b(l)a ([aàâäeéèêëiîïoôöuùüûh])", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x04000EDE RID: 3806
		private Regex ElisionSi = new Regex("\\b(s)i (ils?)\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x04000EDF RID: 3807
		private Regex DeLe = new Regex("\\b(d)e l(es?)\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x04000EE0 RID: 3808
		private Regex ALe = new Regex("\\bà les?\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
	}
}
