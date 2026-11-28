using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022CA RID: 8906
	public struct SlateRef<T> : ISlateRef, IEquatable<SlateRef<T>>
	{
		// Token: 0x17002634 RID: 9780
		// (get) Token: 0x0600DBA7 RID: 56231 RVA: 0x0051DB81 File Offset: 0x0051BD81
		// (set) Token: 0x0600DBA8 RID: 56232 RVA: 0x0051DB89 File Offset: 0x0051BD89
		string ISlateRef.SlateRef
		{
			get
			{
				return this.slateRef;
			}
			set
			{
				this.slateRef = value;
			}
		}

		// Token: 0x0600DBA9 RID: 56233 RVA: 0x0051DB92 File Offset: 0x0051BD92
		public SlateRef(string slateRef)
		{
			this.slateRef = slateRef;
		}

		// Token: 0x0600DBAA RID: 56234 RVA: 0x0051DB9C File Offset: 0x0051BD9C
		public T GetValue(Slate slate)
		{
			T result;
			this.TryGetValue(slate, out result);
			return result;
		}

		// Token: 0x0600DBAB RID: 56235 RVA: 0x0051DBB4 File Offset: 0x0051BDB4
		public bool TryGetValue(Slate slate, out T value)
		{
			return this.TryGetConvertedValue<T>(slate, out value);
		}

		// Token: 0x0600DBAC RID: 56236 RVA: 0x0051DBC0 File Offset: 0x0051BDC0
		public bool TryGetConvertedValue<TAnything>(Slate slate, out TAnything value)
		{
			if (this.slateRef == null)
			{
				value = default(TAnything);
				return true;
			}
			SlateRef<T>.tmpCurSlate = slate;
			string text = SlateRef<T>.HighPriorityVarsRegex.Replace(this.slateRef, new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenate));
			object obj;
			bool flag;
			if (!SlateRefUtility.CheckSingleVariableSyntax(text, slate, out obj, out flag))
			{
				obj = SlateRef<T>.MathExprRegex.Replace(text, SlateRef<T>.RegexMatchEvaluatorEvaluateMathExpressionCached);
				obj = SlateRef<T>.VarsRegex.Replace((string)obj, SlateRef<T>.RegexMatchEvaluatorConcatenateCached);
				flag = true;
			}
			SlateRef<T>.tmpCurSlate = null;
			if (!flag)
			{
				value = default(TAnything);
				return false;
			}
			if (obj == null)
			{
				value = default(TAnything);
				return true;
			}
			if (obj is TAnything)
			{
				value = (TAnything)((object)obj);
				return true;
			}
			if (ConvertHelper.CanConvert<TAnything>(obj))
			{
				value = ConvertHelper.Convert<TAnything>(obj);
				return true;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Could not convert SlateRef \"");
			defaultInterpolatedStringHandler.AppendFormatted(this.slateRef);
			defaultInterpolatedStringHandler.AppendLiteral("\" (");
			defaultInterpolatedStringHandler.AppendFormatted(obj.GetType().Name);
			defaultInterpolatedStringHandler.AppendLiteral(") to ");
			defaultInterpolatedStringHandler.AppendFormatted(typeof(TAnything).Name);
			Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			value = default(TAnything);
			return false;
		}

		// Token: 0x0600DBAD RID: 56237 RVA: 0x0051DCF8 File Offset: 0x0051BEF8
		private static string RegexMatchEvaluatorConcatenate(Match match)
		{
			string value = match.Groups[1].Value;
			object obj;
			if (!SlateRef<T>.tmpCurSlate.TryGet<object>(value, out obj, false))
			{
				return "";
			}
			if (obj == null)
			{
				return "";
			}
			return obj.ToString();
		}

		// Token: 0x0600DBAE RID: 56238 RVA: 0x0051DD3C File Offset: 0x0051BF3C
		private static string RegexMatchEvaluatorConcatenateZeroIfEmpty(Match match)
		{
			string value = match.Groups[1].Value;
			object obj;
			if (!SlateRef<T>.tmpCurSlate.TryGet<object>(value, out obj, false))
			{
				Log.ErrorOnce("Tried to use variable \"" + value + "\" in a math expression but it doesn't exist.", value.GetHashCode() ^ 194857119);
				return "0";
			}
			if (obj == null)
			{
				return "0";
			}
			string text = obj.ToString();
			if (text == "")
			{
				return "0";
			}
			return text;
		}

		// Token: 0x0600DBAF RID: 56239 RVA: 0x0051DDB8 File Offset: 0x0051BFB8
		private static string RegexMatchEvaluatorResolveMathExpression(Match match)
		{
			string text = match.Groups[1].Value;
			text = SlateRef<T>.VarsRegex.Replace(text, SlateRef<T>.RegexMatchEvaluatorConcatenateZeroIfEmptyCached);
			return MathEvaluator.Evaluate(text).ToString();
		}

		// Token: 0x0600DBB0 RID: 56240 RVA: 0x0051DDF6 File Offset: 0x0051BFF6
		public override bool Equals(object obj)
		{
			return obj is SlateRef<T> && this.Equals((SlateRef<T>)obj);
		}

		// Token: 0x0600DBB1 RID: 56241 RVA: 0x0051DE0E File Offset: 0x0051C00E
		public bool Equals(SlateRef<T> other)
		{
			return this == other;
		}

		// Token: 0x0600DBB2 RID: 56242 RVA: 0x0051DE1C File Offset: 0x0051C01C
		public static bool operator ==(SlateRef<T> a, SlateRef<T> b)
		{
			return a.slateRef == b.slateRef;
		}

		// Token: 0x0600DBB3 RID: 56243 RVA: 0x0051DE2F File Offset: 0x0051C02F
		public static bool operator !=(SlateRef<T> a, SlateRef<T> b)
		{
			return !(a == b);
		}

		// Token: 0x0600DBB4 RID: 56244 RVA: 0x0051DE3B File Offset: 0x0051C03B
		public static implicit operator SlateRef<T>(T t)
		{
			return new SlateRef<T>((t != null) ? t.ToString() : null);
		}

		// Token: 0x0600DBB5 RID: 56245 RVA: 0x0051DE5A File Offset: 0x0051C05A
		public override int GetHashCode()
		{
			if (this.slateRef == null)
			{
				return 0;
			}
			return this.slateRef.GetHashCode();
		}

		// Token: 0x0600DBB6 RID: 56246 RVA: 0x0051DE74 File Offset: 0x0051C074
		public string ToString(Slate slate)
		{
			string result;
			this.TryGetConvertedValue<string>(slate, out result);
			return result;
		}

		// Token: 0x0600DBB7 RID: 56247 RVA: 0x0051DE8C File Offset: 0x0051C08C
		public override string ToString()
		{
			if (!QuestGen.Working)
			{
				return this.slateRef;
			}
			return this.ToString(QuestGen.slate);
		}

		// Token: 0x04008CFE RID: 36094
		public const string SlateRefFieldName = "slateRef";

		// Token: 0x04008CFF RID: 36095
		[MustTranslate_SlateRef]
		private string slateRef;

		// Token: 0x04008D00 RID: 36096
		private static Slate tmpCurSlate;

		// Token: 0x04008D01 RID: 36097
		private static readonly Regex VarsRegex = new Regex("\\$([a-zA-Z0-1_/]*)");

		// Token: 0x04008D02 RID: 36098
		private static readonly Regex HighPriorityVarsRegex = new Regex("\\(\\(\\$([a-zA-Z0-1_/]*)\\)\\)");

		// Token: 0x04008D03 RID: 36099
		private static readonly Regex MathExprRegex = new Regex("\\$\\((.*)\\)");

		// Token: 0x04008D04 RID: 36100
		private static MatchEvaluator RegexMatchEvaluatorConcatenateCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenate);

		// Token: 0x04008D05 RID: 36101
		private static MatchEvaluator RegexMatchEvaluatorConcatenateZeroIfEmptyCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenateZeroIfEmpty);

		// Token: 0x04008D06 RID: 36102
		private static MatchEvaluator RegexMatchEvaluatorEvaluateMathExpressionCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorResolveMathExpression);
	}
}
