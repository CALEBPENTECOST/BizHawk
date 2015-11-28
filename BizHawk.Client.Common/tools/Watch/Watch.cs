﻿using BizHawk.Common.NumberExtensions;
using BizHawk.Emulation.Common;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BizHawk.Client.Common
{
	public abstract partial class Watch
	{
		#region Fields

		protected long _address;
		protected MemoryDomain _domain;
		protected DisplayType _type;
		protected WatchSize _size;
		protected bool _bigEndian;
		protected string _notes;
		protected int _changecount = 0;

		#endregion

		#region cTor(s)

		/// <summary>
		/// Initialize a new instance of <see cref="Watch"/>
		/// </summary>
		/// <param name="domain"><see cref="MemoryDomain"/> where you want to track</param>
		/// <param name="address">The address you want to track</param>
		/// <param name="size">A <see cref="WatchSize"/> (byte, word, double word)</param>
		/// <param name="type">How you you want to display the value See <see cref="DisplayType"/></param>
		/// <param name="bigEndian">Specify the endianess. true for big endian</param>
		/// <param name="note">A custom note about the <see cref="Watch"/></param>
		protected Watch(MemoryDomain domain, long address, WatchSize size, DisplayType type, bool bigEndian, string note)
		{
			if (IsDiplayTypeAvailable(type))
			{
				this._domain = domain;
				this._address = address;
				this._size = size;
				this._type = type;
				this._bigEndian = bigEndian;
				this._notes = note;
				return;

			}
			else
			{
				throw new ArgumentException(string.Format("DisplayType {0} is invalid for this type of Watch", type.ToString()), "type");
			}
		}

		#endregion

		#region Methods
		
		#region Static		

		/// <summary>
		/// Generate a <see cref="Watch"/> from a given string
		/// String is tab separate
		/// </summary>
		/// <param name="line">Entire string, tab seperated for each value Order is:
		/// <list type="number">
		/// <item>
		/// <term>0x00</term>
		/// <description>Address in hexadecimal</description>
		/// </item>
		/// <item>
		/// <term>b,w or d</term>
		/// <description>The <see cref="WatchSize"/>, byte, word or double word</description>
		/// <term>s, u, h, b, 1, 2, 3, f</term>
		/// <description>The <see cref="DisplayType"/> signed, unsigned,etc...</description>
		/// </item>
		/// <item>
		/// <term>0 or 1</term>
		/// <description>Big endian or not</description>
		/// </item>
		/// <item>
		/// <term>RDRAM,ROM,...</term>
		/// <description>The <see cref="IMemoryDomains"/></description>
		/// </item>
		/// <item>
		/// <term>Plain text</term>
		/// <description>Notes</description>
		/// </item>
		/// </list>
		/// </param>
		/// <param name="domains"><see cref="Watch"/>'s memory domain</param>
		/// <returns>A brand new <see cref="Watch"/></returns>
		public static Watch FromString(string line, IMemoryDomains domains)
		{
			string[] parts = line.Split(new char[] { '\t' }, 6);

			if (parts.Length < 6)
			{
				if (parts.Length >= 3 && parts[2] == "_")
				{
					return SeparatorWatch.Instance;
				}

				return null;
			}
			long address;

			if (long.TryParse(parts[0], NumberStyles.HexNumber, CultureInfo.CurrentCulture, out address))
			{
				WatchSize size = Watch.SizeFromChar(parts[1][0]);
				DisplayType type = Watch.DisplayTypeFromChar(parts[2][0]);
				bool bigEndian = parts[3] == "0" ? false : true;
				MemoryDomain domain = domains[parts[4]];
				string notes = parts[5].Trim(new char[] { '\r', '\n' });

				return Watch.GenerateWatch(
					domain,
					address,
					size,
					type,
					bigEndian,
					notes
					);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Generate a new <see cref="Watch"/> instance
		/// Can be either <see cref="ByteWatch"/>, <see cref="WordWatch"/>, <see cref="DWordWatch"/> or <see cref="SeparatorWatch"/>
		/// </summary>
		/// <param name="domain">The <see cref="MemoryDomain"/> where you want to watch</param>
		/// <param name="address">The address into the <see cref="MemoryDomain"/></param>
		/// <param name="size">The size</param>
		/// <param name="type">How the watch will be displayed</param>
		/// <param name="bigEndian">Endianess (true for big endian)</param>
		/// <param name="note">A custom note about the <see cref="Watch"/></param>
		/// <param name="value">The current watch value</param>
		/// <param name="prev">Previous value</param>
		/// <param name="changeCount">Number of changes occurs in current <see cref="Watch"/></param>
		/// <returns>New <see cref="Watch"/> instance. True type is depending of size parameter</returns>
		public static Watch GenerateWatch(MemoryDomain domain, long address, WatchSize size, DisplayType type, bool bigEndian, string note, long value, long prev, int changeCount)
		{
			switch (size)
			{
				default:
				case WatchSize.Separator:
					return SeparatorWatch.Instance;
				case WatchSize.Byte:
					return new ByteWatch(domain, address, type, bigEndian, note, (byte)value, (byte)prev, changeCount);
				case WatchSize.Word:
					return new WordWatch(domain, address, type, bigEndian, note, (ushort)value, (ushort)prev, changeCount);
				case WatchSize.DWord:
					return new DWordWatch(domain, address, type, bigEndian, note, (uint)value, (uint)prev, changeCount);
			}
		}

		/// <summary>
		/// Generate a new <see cref="Watch"/> instance
		/// Can be either <see cref="ByteWatch"/>, <see cref="WordWatch"/>, <see cref="DWordWatch"/> or <see cref="SeparatorWatch"/>
		/// </summary>
		/// <param name="domain">The <see cref="MemoryDomain"/> where you want to watch</param>
		/// <param name="address">The address into the <see cref="MemoryDomain"/></param>
		/// <param name="size">The size</param>
		/// <param name="type">How the watch will be displayed</param>
		/// <param name="bigEndian">Endianess (true for big endian)</param>
		/// <param name="note">A customp note about your watch</param>
		/// <returns>New <see cref="Watch"/> instance. True type is depending of size parameter</returns>
		public static Watch GenerateWatch(MemoryDomain domain, long address, WatchSize size, DisplayType type, bool bigEndian, string note)
		{
			return GenerateWatch(domain, address, size, type, bigEndian, note, 0, 0, 0);
		}

		/// <summary>
		/// Generate a new <see cref="Watch"/> instance
		/// Can be either <see cref="ByteWatch"/>, <see cref="WordWatch"/>, <see cref="DWordWatch"/> or <see cref="SeparatorWatch"/>
		/// </summary>
		/// <param name="domain">The <see cref="MemoryDomain"/> where you want to watch</param>
		/// <param name="address">The address into the <see cref="MemoryDomain"/></param>
		/// <param name="size">The size</param>
		/// <param name="type">How the watch will be displayed</param>
		/// <param name="bigEndian">Endianess (true for big endian)</param>
		/// <returns>New <see cref="Watch"/> instance. True type is depending of size parameter</returns>
		public static Watch GenerateWatch(MemoryDomain domain, long address, WatchSize size, DisplayType type, bool bigEndian)
		{
			return GenerateWatch(domain, address, size, type, bigEndian, string.Empty, 0, 0, 0);
		}

		/// <summary>
		/// Equality operator between two <see cref="Watch"/>
		/// </summary>
		/// <param name="a">First watch</param>
		/// <param name="b">Second watch</param>
		/// <returns>True if both watch are equals; otherwise, false</returns>
		public static bool operator ==(Watch a, Watch b)
		{
			if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
			{
				return false;
			}
			else if (object.ReferenceEquals(a, b))
			{
				return true;
			}
			else
			{
				return a.Equals(b);
			}
		}

		/// <summary>
		/// Equality operator between a <see cref="Watch"/> and a <see cref="Cheat"/>
		/// </summary>
		/// <param name="a">The watch</param>
		/// <param name="b">The cheat</param>
		/// <returns>True if they are equals; otherwise, false</returns>
		public static bool operator ==(Watch a, Cheat b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Inequality operator between two <see cref="Watch"/>
		/// </summary>
		/// <param name="a">First watch</param>
		/// <param name="b">Second watch</param>
		/// <returns>True if both watch are different; otherwise, false</returns>
		public static bool operator !=(Watch a, Watch b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Inequality operator between a <see cref="Watch"/> and a <see cref="Cheat"/>
		/// </summary>
		/// <param name="a">The watch</param>
		/// <param name="b">The cheat</param>
		/// <returns>True if they are different; otherwise, false</returns>
		public static bool operator !=(Watch a, Cheat b)
		{
			return !(a == b);
		}

		#endregion Static

		#region Abstracts

		/// <summary>
		/// Get a list a <see cref="DisplayType"/> that can be used for this <see cref="Watch"/>
		/// </summary>
		/// <returns>An enumartion that contains all valid <see cref="DisplayType"/></returns>
		public abstract IEnumerable<DisplayType> AvailableTypes();

		/// <summary>
		/// Reset the previous value
		/// </summary>
		public abstract void ResetPrevious();

		/// <summary>
		/// Update the Watch (read it from <see cref="MemoryDomain"/>
		/// </summary>
		public abstract void Update();

		#endregion Abstracts

		#region Protected

		protected byte GetByte(bool bypassFreeze = false)
		{
			if (!bypassFreeze && Global.CheatList.IsActive(_domain, _address))
			{
				//LIAR logic
				return Global.CheatList.GetByteValue(_domain, _address).Value;
			}
			else
			{
				if (_domain.Size == 0)
				{
					return _domain.PeekByte(_address);
				}
				else
				{
					return _domain.PeekByte(_address % _domain.Size);
				}
			}
		}

		protected ushort GetWord(bool bypassFreeze = false)
		{
			if (!bypassFreeze && Global.CheatList.IsActive(_domain, _address))
			{
				//LIAR logic
				return (ushort)Global.CheatList.GetCheatValue(_domain, _address, WatchSize.Word).Value;
			}
			else
			{
				if (_domain.Size == 0)
				{
					return _domain.PeekWord(_address, _bigEndian);
				}
				else
				{
					return _domain.PeekWord(_address % _domain.Size, _bigEndian); // TODO: % size stil lisn't correct since it could be the last byte of the domain
				}
			}
		}

		protected uint GetDWord(bool bypassFreeze = false)
		{
			if (!bypassFreeze && Global.CheatList.IsActive(_domain, _address))
			{
				//LIAR logic
				return (uint)Global.CheatList.GetCheatValue(_domain, _address, WatchSize.DWord).Value;
			}
			else
			{
				if (_domain.Size == 0)
				{
					return _domain.PeekDWord(_address, _bigEndian); // TODO: % size stil lisn't correct since it could be the last byte of the domain
				}
				else
				{
					return _domain.PeekDWord(_address % _domain.Size, _bigEndian); // TODO: % size stil lisn't correct since it could be the last byte of the domain
				}
			}
		}

		protected void PokeByte(byte val)
		{
			if (_domain.Size == 0)
				_domain.PokeByte(_address, val);
			else _domain.PokeByte(_address % _domain.Size, val);
		}

		protected void PokeWord(ushort val)
		{
			if (_domain.Size == 0)
				_domain.PokeWord(_address, val, _bigEndian); // TODO: % size stil lisn't correct since it could be the last byte of the domain
			else _domain.PokeWord(_address % _domain.Size, val, _bigEndian); // TODO: % size stil lisn't correct since it could be the last byte of the domain
		}

		protected void PokeDWord(uint val)
		{
			if (_domain.Size == 0)
				_domain.PokeDWord(_address, val, _bigEndian); // TODO: % size stil lisn't correct since it could be the last byte of the domain
			else _domain.PokeDWord(_address % _domain.Size, val, _bigEndian); // TODO: % size stil lisn't correct since it could be the last byte of the domain
		}

		#endregion Protected

		/// <summary>
		/// Set the number of changes to 0
		/// </summary>
		public void ClearChangeCount()
		{
			_changecount = 0;
		}

		/// <summary>
		/// Determine if this object is Equals to another
		/// </summary>
		/// <param name="obj">The object to compare</param>
		/// <returns>True if both object are equals; otherwise, false</returns>
		public override bool Equals(object obj)
		{
			Watch watch = (Watch)obj;

			if (obj is Watch)
			{
				return this.Domain == watch.Domain &&
					this.Address == watch.Address &&
					this.Size == watch.Size &&
					this.Type == watch.Type &&
					this.Notes == watch.Notes;
			}
			else if (obj is Cheat)
			{
				return this.Domain == watch.Domain && this.Address == watch.Address;
			}
			else
			{
				return base.Equals(obj);
			}
		}

		/// <summary>
		/// Hash the current watch and gets a unique value
		/// </summary>
		/// <returns>int that can serves as a unique representation of current Watch</returns>
		public override int GetHashCode()
		{
			return this.Domain.GetHashCode() + (int)(this.Address ?? 0);
		}

		/// <summary>
		/// Determine if the specified <see cref="DisplayType"/> can be
		/// used for the current <see cref="Watch"/>
		/// </summary>
		/// <param name="type"><see cref="DisplayType"/> you want to check</param>
		/// <returns></returns>
		public bool IsDiplayTypeAvailable(DisplayType type)
		{
			return AvailableTypes().Where<DisplayType>(d => d == type).Any<DisplayType>();
        }

		/// <summary>
		/// Transform the current instance into a string
		/// </summary>
		/// <returns>A <see cref="string"/> representation of the current <see cref="Watch"/></returns>
		public override string ToString()
		{
			return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}"
				, (Address ?? 0).ToHexString((Domain.Size - 1).NumHexDigits())
				, SizeAsChar
				, TypeAsChar
				, BigEndian
				, DomainName
				, Notes.Trim('\r', '\n')
				);
		}

		#endregion

		#region Properties

		#region Abstracts

		public abstract string Diff { get; }
		public abstract uint MaxValue { get; }
		public abstract int? Value { get; }
		//zero 15-nov-2015 - bypass LIAR LOGIC, see fdc9ea2aa922876d20ba897fb76909bf75fa6c92 https://github.com/TASVideos/BizHawk/issues/326
		public abstract int? ValueNoFreeze { get; }
		public abstract string ValueString { get; }
		public abstract bool Poke(string value);
		public abstract int? Previous { get; }
		public abstract string PreviousStr { get; }

		#endregion Abstracts

		/// <summary>
		/// Gets the address in the <see cref="MemoryDomain"/>
		/// </summary>
		public long? Address
		{
			get
			{
				return _address;
			}
		}

		/// <summary>
		/// Gets the format tha should be used by string.Format()
		/// </summary>
		private string AddressFormatStr
		{
			get
			{
				if (_domain != null)
				{
					return "X" + (_domain.Size - 1).NumHexDigits();
				}

				return string.Empty;
			}
		}

		/// <summary>
		/// Gets the address in the <see cref="MemoryDomain"/> formatted as string
		/// </summary>
		public string AddressString
		{
			get
			{
				return _address.ToString(AddressFormatStr);
			}
		}

		/// <summary>
		/// Gets or sets the endianess of current <see cref="Watch"/>
		/// True for big endian, flase for little endian
		/// </summary>
		public bool BigEndian
		{
			get
			{
				return _bigEndian;
			}
			set
			{
				_bigEndian = value;
			}
		}

		/// <summary>
		/// Gets the number of time tha value of current <see cref="Watch"/> has changed
		/// </summary>
		public int ChangeCount
		{
			get
			{
				return _changecount;
			}
		}

		/// <summary>
		/// Gets or set the way current <see cref="Watch"/> is displayed
		/// </summary>
		public DisplayType Type
		{
			get
			{
				return _type;
			}
			set
			{
				if (IsDiplayTypeAvailable(value))
				{
					_type = value;
				}
				else
				{
					throw new ArgumentException(string.Format("DisplayType {0} is invalid for this type of Watch", value.ToString()));
				}
			}
		}

		/// <summary>
		/// Gets or sets current <see cref="MemoryDomain"/>
		/// </summary>
		public MemoryDomain Domain
		{
			get
			{
				return _domain;
			}
			set
			{
				_domain = value;
			}
		}

		/// <summary>
		/// Gets the domain name of the current <see cref="MemoryDomain"/>
		/// It's the same of doing myWatch.Domain.Name
		/// </summary>
		public string DomainName
		{
			get
			{
				if (_domain != null)
				{
					return _domain.Name;
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Gets a value that defined if the current address is 
		/// well in the range of current <see cref="MemoryDomain"/>
		/// </summary>
		public bool IsOutOfRange
		{
			get
			{
				return !IsSeparator && (_domain.Size != 0 && _address >= _domain.Size);
			}
		}

		/// <summary>
		/// Gets a value that defined if the current <see cref="Watch"/> is actually a <see cref="SeparatorWatch"/>
		/// </summary>
		public bool IsSeparator
		{
			get
			{
				return this is SeparatorWatch;
			}
		}

		/// <summary>
		/// Gets or sets notes for current <see cref="Watch"/>
		/// </summary>
		public string Notes
		{
			get
			{
				return _notes;
			}
			set
			{
				_notes = value;
			}
		}

		/// <summary>
		/// Gets the current size of the watch
		/// </summary>
		public WatchSize Size
		{
			get
			{
				return _size;
			}
		}

		#endregion

		public static string DisplayTypeToString(DisplayType type)
		{
			switch (type)
			{
				default:
					return type.ToString();
				case DisplayType.FixedPoint_12_4:
					return "Fixed Point 12.4";
				case DisplayType.FixedPoint_20_12:
					return "Fixed Point 20.12";
				case DisplayType.FixedPoint_16_16:
					return "Fixed Point 16.16";
			}
		}

		public static DisplayType StringToDisplayType(string name)
		{
			switch (name)
			{
				default:
					return (DisplayType)Enum.Parse(typeof(DisplayType), name);
				case "Fixed Point 12.4":
					return DisplayType.FixedPoint_12_4;
				case "Fixed Point 20.12":
					return DisplayType.FixedPoint_20_12;
				case "Fixed Point 16.16":
					return DisplayType.FixedPoint_16_16;
			}
		}

		public char SizeAsChar
		{
			get
			{
				switch (Size)
				{
					default:
					case WatchSize.Separator:
						return 'S';
					case WatchSize.Byte:
						return 'b';
					case WatchSize.Word:
						return 'w';
					case WatchSize.DWord:
						return 'd';
				}
			}
		}

		public static WatchSize SizeFromChar(char c)
		{
			switch (c)
			{
				default:
				case 'S':
					return WatchSize.Separator;
				case 'b':
					return WatchSize.Byte;
				case 'w':
					return WatchSize.Word;
				case 'd':
					return WatchSize.DWord;
			}
		}

		public char TypeAsChar
		{
			get
			{
				switch (Type)
				{
					default:
					case DisplayType.Separator:
						return '_';
					case DisplayType.Unsigned:
						return 'u';
					case DisplayType.Signed:
						return 's';
					case DisplayType.Hex:
						return 'h';
					case DisplayType.Binary:
						return 'b';
					case DisplayType.FixedPoint_12_4:
						return '1';
					case DisplayType.FixedPoint_20_12:
						return '2';
					case DisplayType.FixedPoint_16_16:
						return '3';
					case DisplayType.Float:
						return 'f';
				}
			}
		}

		public static DisplayType DisplayTypeFromChar(char c)
		{
			switch (c)
			{
				default:
				case '_':
					return DisplayType.Separator;
				case 'u':
					return DisplayType.Unsigned;
				case 's':
					return DisplayType.Signed;
				case 'h':
					return DisplayType.Hex;
				case 'b':
					return DisplayType.Binary;
				case '1':
					return DisplayType.FixedPoint_12_4;
				case '2':
					return DisplayType.FixedPoint_20_12;
				case '3':
					return DisplayType.FixedPoint_16_16;
				case 'f':
					return DisplayType.Float;
			}
		}
	}
}