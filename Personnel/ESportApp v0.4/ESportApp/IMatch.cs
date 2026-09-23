namespace ESportApp
{
    // Les trois jeux ont des statistiques propres (agent, map, champion, CS...),
    // mais ils partagent un socle commun. Cette interface décrit ce socle :
    // elle permet d'écrire UNE seule fois les prédicats (--filter) et les
    // sélecteurs (--stat), au lieu d'un jeu de tables par jeu.
    //
    // `Timestamp` en fait partie : un match est daté par lui-même, la série se
    // contente de les enfiler. C'est ce qui permet à DataSerie<T> de rester un
    // simple IEnumerable<T>, sans emballage supplémentaire.
    //
    // Elle vit dans EsportApp, pas dans DataSeries : la bibliothèque doit rester
    // ignorante du domaine.
    public interface IMatch
    {
        DateTime Timestamp { get; }
        string Player { get; }
        int Kills { get; }
        int Deaths { get; }
        int Assists { get; }
        bool Won { get; }
    }
}
