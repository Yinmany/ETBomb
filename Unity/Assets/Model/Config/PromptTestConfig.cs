using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
	[Config]
	public partial class PromptTestConfigCategory : ACategory<PromptTestConfig>
	{
		public static PromptTestConfigCategory Instance;
		public PromptTestConfigCategory()
		{
			Instance = this;
		}
	}

	public partial class PromptTestConfig: IConfig
	{
		[BsonId]
		public long Id { get; set; }
		public string Target;
		public string TargetType;
		public string Cards;
		public int Result;
		public string Handles;
		public string Prompt;
	}
}
