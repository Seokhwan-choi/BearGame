using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bear
{
	public static class LJson
	{
		static JsonSerializerSettings mSetting = CreateSetting();

		public static string ToJson(object obj, bool pretty = false, JsonSerializerSettings setting = null)
		{
			return JsonConvert.SerializeObject(obj, pretty ? Formatting.Indented : Formatting.None, setting != null ? setting : mSetting);
		}

		public static string ToJson(object obj, bool pretty)
		{
			return ToJson(obj, pretty, mSetting);
		}

		public static object FromJson(string json, Type type)
		{
			return JsonConvert.DeserializeObject(json, type, mSetting);
		}

		public static T FromJson<T>(string json, JsonSerializerSettings settings = null)
		{
			return (T)JsonConvert.DeserializeObject(json, typeof(T), settings ?? mSetting);
		}

		public static T DeepCopy<T>(T obj)
		{
			string json = ToJson(obj);

			return FromJson<T>(json);
		}

		public static JsonSerializerSettings CreateSetting(IContractResolver contractResolver = null)
		{
			return new JsonSerializerSettings()
			{
				Converters = CreateLmgLibConverters(),
				ContractResolver = contractResolver != null ? contractResolver : new LJsonDefaultResolver(),
				DefaultValueHandling = DefaultValueHandling.Ignore,
			};
		}

		static public JsonConverter[] LmgLibConvertes = CreateLmgLibConverters();

		static JsonConverter[] CreateLmgLibConverters()
		{
			return new JsonConverter[]
			{
				new enum_reader()
			};
		}

#if UNITY_EDITOR
		[UnityEditor.InitializeOnLoadMethod]
		static void EditorInit()
		{
			LmgLibConvertes = CreateLmgLibConverters();
			mSetting = CreateSetting();
		}
#endif
	}

	class LJsonDefaultResolver : DefaultContractResolver
	{

		public LJsonDefaultResolver(bool ignoreSymbol = false)
		{
		}

		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);

			var propList = props.Where(p => p.Writable).ToArray();

			return propList;
		}

	}
	static class JsonPropertyExt
	{
		public static bool HaveAttribute<T>(this JsonProperty property) where T : Attribute
		{
			IList<Attribute> attrList = property.AttributeProvider.GetAttributes(typeof(T), false);

			return attrList.Count > 0;
		}
	}

	class LJsonResolver : DefaultContractResolver
	{
		public LJsonResolver()
		{
		}

		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);

			return props.Where(p => p.Writable).ToArray();
		}
	}


	// Converters
	//////////////////////////////////////////////////////////////////////////
	public class enum_reader : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType.IsEnum;
		}

		public override bool CanWrite { get { return false; } }

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			try
			{
				if (reader.TokenType == JsonToken.Integer)
				{
					return Enum.ToObject(objectType, reader.Value);
				}
				else if (reader.TokenType == JsonToken.String)
				{
					string name = reader.Value.ToString();

					return Enum.Parse(objectType, name);
				}
			}
			catch
			{
			}

			return Activator.CreateInstance(objectType);
		}
	}
}