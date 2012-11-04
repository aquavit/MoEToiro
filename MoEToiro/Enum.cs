using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoEABCPlay
{
	/// <summary>  
	/// MMRESULTのマネージド実装です。  
	/// </summary>  
	internal enum MMResult : uint
	{
		/// <summary>  
		/// 処理に成功しました。  
		/// </summary>  
		NoError = 0,
		/// <summary>  
		/// 指定されたIDは無効です。  
		/// </summary>  
		InvalidDeviceID = 2,
		/// <summary>  
		/// 指定されたリソースは既に割り当てられています。  
		/// </summary>  
		Allocated = 4
	}

	/// <summary>  
	/// MIDIポートの種類を表すフラグです。  
	/// </summary>  
	internal enum MidiModuleType : ushort
	{
		/// <summary>  
		/// このポートはハードウェアポートです。  
		/// </summary>  
		Hardware = 1,
		/// <summary>  
		/// このポートはソフトウェアシンセサイザです。  
		/// </summary>  
		Synthesizer = 2,
		/// <summary>  
		/// このポートは矩形シンセサイザです。  
		/// </summary>  
		SquareSynth = 3,
		/// <summary>  
		/// このポートはFMシンセサイザです。  
		/// </summary>  
		FMSynth = 4,
		/// <summary>  
		/// このポートはMIDIマッパーです。  
		/// </summary>  
		MidiMapper = 5,
		/// <summary>  
		/// このポートはウェーブテーブルシンセサイザです。  
		/// </summary>  
		Wavetable = 6,
		/// <summary>  
		/// このポートはソフトウェアシンセサイザです。  
		/// </summary>  
		SoftwareSynth = 7
	}

	/// <summary>  
	/// MIDIポートの能力を示すフラグです。  
	/// </summary>  
	[Flags]
	internal enum MidiPortCapability : uint
	{
		/// <summary>  
		/// ポートはボリュームコントロールをサポートします。  
		/// </summary>  
		Volume = 1,
		/// <summary>  
		/// ポートは左右独立のボリュームコントロールをサポートします。  
		/// </summary>  
		LRVolume = 2,
		/// <summary>  
		/// ポートはキャッシュをサポートします。  
		/// </summary>  
		Cache = 4,
		/// <summary>  
		/// ポートはMIDIストリームAPIをネイティブサポートします。  
		/// </summary>  
		Stream = 8
	}

	/// <summary>  
	/// MIDIポートを開く時のオプションです。  
	/// </summary>  
	internal enum MidiPortOpenFlag : uint
	{
		/// <summary>  
		/// コールバック機構を使用しません。  
		/// </summary>  
		NoCallback = 0,
		/// <summary>  
		/// コールバックはウィンドウメッセージとして送信されます。  
		/// </summary>  
		CallbackWindow = 0x10000,
		/// <summary>  
		/// コールバックはスレッドに送信されます。  
		/// </summary>  
		CallbackThread = 0x20000,
		/// <summary>  
		/// コールバックは関数ポインタです。  
		/// </summary>  
		CallbackFunction = 0x30000
	}

	/// <summary>  
	/// MidiHdrのdwFlags値を表す列挙子です。  
	/// </summary>  
	[Flags]
	internal enum MidiHdrFlag : uint
	{
		/// <summary>  
		/// フラグがセットされていません。  
		/// </summary>  
		None = 0,
		/// <summary>  
		/// バッファの使用が完了しました。  
		/// </summary>  
		Done = 1,
		/// <summary>  
		/// バッファの準備が完了しました。  
		/// </summary>  
		Prepared = 2,
		/// <summary>  
		/// バッファは再生待ちです。  
		/// </summary>  
		InQueue = 4
	}

	/// <summary>
	/// Melodic sounds 列挙子
	/// </summary>
	public enum GMProgram : byte
	{
		//Piano
		AcousticPiano,
		BrightPiano,
		ElectricGrandPiano,
		Honky_tonkPiano,
		ElectricPiano,
		ElectricPiano2,
		Harpsichord,
		Clavi,
		//ChromaticPercussion
		Celesta,
		Glockenspiel,
		Musicalbox,
		Vibraphone,
		Marimba,
		Xylophone,
		TubularBell,
		Dulcimer,
		//Organ
		DrawbarOrgan,
		PercussiveOrgan,
		RockOrgan,
		Churchorgan,
		Reedorgan,
		Accordion,
		Harmonica,
		TangoAccordion,
		//Guitar
		AcousticGuitar_nylon,
		AcousticGuitar_steel,
		ElectricGuitar_jazz,
		ElectricGuitar_clean,
		ElectricGuitar_muted,
		OverdrivenGuitar,
		DistortionGuitar,
		Guitarharmonics,
		//Bass
		AcousticBass,
		ElectricBass_finger,
		ElectricBass_pick,
		FretlessBass,
		SlapBass1,
		SlapBass2,
		SynthBass1,
		SynthBass2,
		//Strings
		Violin,
		Viola,
		Cello,
		Doublebass,
		TremoloStrings,
		PizzicatoStrings,
		OrchestralHarp,
		Timpani,
		//Ensemble
		StringEnsemble1,
		StringEnsemble2,
		SynthStrings1,
		SynthStrings2,
		VoiceAahs,
		VoiceOohs,
		SynthVoice,
		OrchestraHit,
		//Brass
		Trumpet,
		Trombone,
		Tuba,
		MutedTrumpet,
		Frenchhorn,
		BrassSection,
		SynthBrass1,
		SynthBrass2,
		//Reed
		SopranoSax,
		AltoSax,
		TenorSax,
		BaritoneSax,
		Oboe,
		EnglishHorn,
		Bassoon,
		Clarinet,
		//Pipe
		Piccolo,
		Flute,
		Recorder,
		PanFlute,
		BlownBottle,
		Shakuhachi,
		Whistle,
		Ocarina,
		//SynthLead
		Lead1_square,
		Lead2_sawtooth,
		Lead3_calliope,
		Lead4_chiff,
		Lead5_charang,
		Lead6_voice,
		Lead7_fifths,
		Lead8_bass_lead,
		//SynthPad
		Pad1_Fantasia,
		Pad2_warm,
		Pad3_polysynth,
		Pad4_choir,
		Pad5_bowed,
		Pad6_metallic,
		Pad7_halo,
		Pad8_sweep,
		//SynthEffects
		FX1_rain,
		FX2_soundtrack,
		FX3_crystal,
		FX4_atmosphere,
		FX5_brightness,
		FX6_goblins,
		FX7_echoes,
		FX8_sci_fi,
		//Ethnic
		Sitar,
		Banjo,
		Shamisen,
		Koto,
		Kalimba,
		Bagpipe,
		Fiddle,
		Shanai,
		//Percussive
		TinkleBell,
		Agogo,
		SteelDrums,
		Woodblock,
		TaikoDrum,
		MelodicTom,
		SynthDrum,
		ReverseCymbal,
		//Soundeffects
		GuitarFretNoise,
		BreathNoise,
		Seashore,
		BirdTweet,
		TelephoneRing,
		Helicopter,
		Applause,
		Gunshot,
	}

}
