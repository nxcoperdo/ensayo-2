using UnityEngine;

public class SonidosJuego : MonoBehaviour
{
    public static SonidosJuego Instance;

    [Header("Soldado")]
    public AudioClip punio;
    public AudioClip patada;
    public AudioClip danoRecibido;
    public AudioClip curacion;

    [Header("Slender")]
    public AudioClip slenderAparece;
    public AudioClip slenderAtaque;

    [Header("Juego")]
    public AudioClip victoria;
    public AudioClip derrota;
    public AudioClip musicaTension;

    [Range(0f, 1f)] public float volumen = 0.8f;
    [Range(0f, 1f)] public float volumenMusica = 0.4f;

    [Header("Tono de la patada")]
    public float pitchPatada = 0.8f; // menor = mas grave

    AudioSource fx;
    AudioSource fxPatada;
    AudioSource musica;
    AudioSource loopCuracion;

    void Awake()
    {
        Instance = this;

        fx = gameObject.AddComponent<AudioSource>();
        fx.playOnAwake = false;

        // fuente aparte para la patada, asi su tono no afecta a los demas sonidos
        fxPatada = gameObject.AddComponent<AudioSource>();
        fxPatada.playOnAwake = false;
        fxPatada.pitch = pitchPatada;

        musica = gameObject.AddComponent<AudioSource>();
        musica.loop = true;
        musica.playOnAwake = false;
        musica.volume = volumenMusica;

        loopCuracion = gameObject.AddComponent<AudioSource>();
        loopCuracion.loop = true;
        loopCuracion.playOnAwake = false;
        loopCuracion.clip = curacion;
        loopCuracion.volume = volumen * 0.6f;
    }

    public void Play(AudioClip clip)
    {
        if (clip != null) fx.PlayOneShot(clip, volumen);
    }

    public void PlayPunio() { Play(punio); }

    public void PlayPatada()
    {
        if (patada != null) fxPatada.PlayOneShot(patada, volumen);
    }

    public void PlayDano() { Play(danoRecibido); }
    public void PlaySlenderAparece() { Play(slenderAparece); }
    public void PlaySlenderAtaque() { Play(slenderAtaque); }
    public void PlayVictoria() { DetenerMusica(); Play(victoria); }
    public void PlayDerrota() { DetenerMusica(); Play(derrota); }

    public void IniciarMusica()
    {
        if (musicaTension == null) return;
        musica.clip = musicaTension;
        musica.Play();
    }

    public void DetenerMusica()
    {
        musica.Stop();
        loopCuracion.Stop();
    }

    public void Curando(bool activo)
    {
        if (loopCuracion.clip == null) return;
        if (activo && !loopCuracion.isPlaying) loopCuracion.Play();
        else if (!activo && loopCuracion.isPlaying) loopCuracion.Stop();
    }
}