using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Commerce; 
 
 
public class MvtDetaillee : Mouvement {
    string nomarticle;
    int typeachat;
    string idese;
    string background;

    public string Nomarticle
    {
        get { return nomarticle; }
        set { nomarticle = value; }
    }
    public int Typeachat
    {
        get { return typeachat; }
        set { typeachat = value; }
    }
    public string Idese
    {
        get { return idese; }
        set { idese = value; }
    }

    public string Background
    {
        get { return background; }
        set { background = value; }
    }

    public MvtDetaillee() { }

    public MvtDetaillee(string id, string idarticle, string nomarticle, int typeachat, string idese, 
        double entrer, double sortie, double prixunitaire, DateTime dates, int etatmvt) : base(id,idarticle,entrer,
            sortie,prixunitaire,dates,etatmvt)
    {
        Nomarticle = nomarticle;
        Typeachat = typeachat;
        Idese = idese;
    }

    public MvtDetaillee(string idarticle, string nomarticle, int typeachat, string idese,
        double entrer, double sortie, double prixunitaire, DateTime dates, int etatmvt) : base(idarticle, entrer,
            sortie, prixunitaire, dates, etatmvt)
    {
        Nomarticle = nomarticle;
        Typeachat = typeachat;
        Idese = idese;
    }
}