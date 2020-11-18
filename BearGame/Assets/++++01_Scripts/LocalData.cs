using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Bear
{
	public class LocalData
	{
		public ulong BitFlags;

		public int TotalGold;

		public Dictionary<string, int> Upgrades;
        public Dictionary<int, FishData> FishIndex; // 물고기 도감, key : 지역, value : 물고기들

		// Methods
		public void Save() { LocalDataSerializer.Save(this); }
		public void Init()
		{
			if (Upgrades == null)
				Upgrades = new Dictionary<string, int>();

			if (FishIndex == null)
				FishIndex = new Dictionary<int, FishData>();
		}
	}


	// LocalData 클래스를 변수 위주로 깔끔하게 하기 위해 Serializer를 별도 클래스로 분리
	static class LocalDataSerializer
	{
		const bool UseCompression = false;
		const string SerialPrefKey = "okfgpejnbdblk";           // 아무 의미없는 값
		const string DataPrefKey = "834mjfboeds89";             // 아무 의미없는 값

		static byte[] mCipherKey;
		static public byte[] CipherKey { get { return mCipherKey; } }

		static public void InitSerial()
		{
			string serial = PlayerPrefs.GetString(SerialPrefKey);
			if (serial == string.Empty)
			{
				serial = "주석" + Util.RandString(16);
				PlayerPrefs.SetString(SerialPrefKey, serial);
			}

			mCipherKey = Encoding.UTF8.GetBytes(serial);
		}

		public static LocalData Load()
		{
			InitSerial();

			try
			{
				string base64 = PlayerPrefs.GetString(DataPrefKey, null);
				if (base64 != string.Empty)
				{
					byte[] bytes = Convert.FromBase64String(base64);

					LocalData data = LCipher.DecodeObject<LocalData>(bytes, mCipherKey, UseCompression);
					if (data != null)
					{
						return data;
					}
				}
			}
			catch
			{
			}

			Debug.Log("LocalData: Generate new local data");

			return MakeNewLocalData(null);
		}

		static LocalData MakeNewLocalData(LocalData prev)
		{
			var newData = new LocalData();

			return newData;
		}

		public static void Save(LocalData data)
		{
			byte[] bytes = LCipher.EncodeObject(data, mCipherKey, UseCompression);

			string base64 = Convert.ToBase64String(bytes);

			PlayerPrefs.SetString(DataPrefKey, base64);
		}

		public static void Reset()
		{
			var prev = Bear.LocalData;

			Bear.LocalData = MakeNewLocalData(prev);

			Debug.Log("LocalData: Reset");
		}

		public static void Delete()
		{
			PlayerPrefs.DeleteKey(DataPrefKey);
		}
	}

	//순서가 변경되면 안 됨.
	public enum LocalBitFlags
	{
		Count,
	}

	static class LocalBitFlagsExt
	{
		public static void Set(this LocalBitFlags flag, bool value = true)
		{
			ulong mask = 1UL << (int)flag;

			if (value)
				Bear.LocalData.BitFlags |= mask;
			else
				Bear.LocalData.BitFlags &= ~mask;

			Bear.LocalData.Save();
		}

		public static bool Get(this LocalBitFlags flag)
		{
			ulong mask = 1UL << (int)flag;
			return (Bear.LocalData.BitFlags & mask) != 0;
		}

		public static void Toggle(this LocalBitFlags flag)
		{
			flag.Set(!flag.Get());
		}

		public static bool IsOn(this LocalBitFlags flag)
		{
			return flag.Get();
		}

		public static bool IsOff(this LocalBitFlags flag)
		{
			return flag.Get() == false;
		}
	}
}