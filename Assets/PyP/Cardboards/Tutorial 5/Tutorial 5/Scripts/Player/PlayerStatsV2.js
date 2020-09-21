//IMPORTANT NOTE! THIS SCRIPT WAS MADE IN VIDEO NUMBER 21! Check out PlayerStats to use the old one.

#pragma strict

var MaxHealth = 100;
var Health : int;
var maximumHitPoints = 100;
var hitPoints : float;
var regenerationSpeed : float = 5;

var damageSound : AudioClip[];
private var edamageSound : int = 0;

var playSoundsGO : GameObject;
var shakeGO : GameObject;
var die : AudioClip;
var deadReplacement : Transform;
var mySkin : GUISkin;
var canRegenerate : boolean = true;

private var radar : GameObject;
private var maxHitPoints : int;
var damageTexture : Texture;

private var time : float = 0.0;
private var alpha : float;
private var callFunction : boolean = false;
private var showTexture : boolean = false;
private var damageMessage : boolean = false;



function Start (temp)
{
	Health = MaxHealth;
	GUI.Box (new Rect (0,0,100,50), "Top-left");
		
	
	
}

function ApplyDammage (TheDammage : int)
{
	Health -= TheDammage;
		playSoundsGO.audio.clip = damageSound[Random.Range(0, damageSound.length)];
		playSoundsGO.audio.volume = 0.4;
		playSoundsGO.audio.Play();
		time += TheDammage/7;

		if(Health <= 0)
	{
		Dead();
	}
}

function Dead()
{
	RespawnMenuV2.playerIsDead = true; //VERY IMPORTANT! This line was added in tutorial number 19. If you haven't reached that tutorial yet, go ahead and remove it.
	Debug.Log("Player Died");
}

function RespawnStats ()
{
	Health = MaxHealth;
}

