import { AccountModel } from './Account';
import { VideoModel } from './Video';
import { ArticleModel, ArticleRowModel } from './Article';
import { TargetModel } from './Target';

//
//  The classes here should match webapi models
//

export class HomePageModel {

    public Videos: VideoModel[] = [];
    public Statistics: NewsStatisticsModel[] = [];
    public Articles: ArticleRowModel[] = [];
    public Warnings: SiteWarningModel[] = [];
    
    public NumTargets: number = 0;
    public NumApproval: number = 0;
    public NumRevision: number = 0;
    public NumNarration: number = 0;
    public NumProduction: number = 0;

    public Seq: number = 0;

}

export class SiteWarningModel {

    public Type: number = 0;
    public Title: string = "";
    public Text: string = "";

}

export class MainPageModel {

    public Targets: TargetModel[] = [];
    public Articles: ArticleModel[] = [];
    public Videos: VideoModel[] = [];

    public NumArticles: number = 0;
    public NumShortNote: number = 0;
    public NumSatoshis: number = 0;

}

export class NewsStatisticsModel {

    public Realm: string = "";
    public Parameter: string = "";
    public ImageLink: string = "";
    public Value: number = 0;
    public Units: string = "";
    public Link: string = "";

}

export class UserIdModel {

    public Id: string = "";
    public Name: string = "";
    
}

export class NewsCategory {

    public Label: string = "";
    public Category: string = "";

}

export class NewsAward {

    public Id: string = "";
    public Name: string = "";
    public Description: string = "";

}

export class CategInfo {

    public MainCategory: NewsCategory = { Label: "", Category: "" };
    public Categories: NewsCategory[] = [];

}

export class AuthorInfo {
    
    public SuggestedLabel: string = "";
    public AuthoredLabel: string = "";
    public RevisedLabel: string = "";
    public NarratedLabel: string = "";
    public ProducedLabel: string = "";

    public Suggested: UserIdModel = { Id: "", Name: "" };
    public Authored: UserIdModel = { Id: "", Name: "" };
    public Revised: UserIdModel = { Id: "", Name: "" };
    public Narrated: UserIdModel = { Id: "", Name: "" };
    public Produced: UserIdModel = { Id: "", Name: "" };

    public DateLabel: string = "";
    public Date: string = "";

    public StatusText: string = "";

}

export class ListItem {

    public Id: string = "";
    public Text: string = "";

}

export class UserPageModel {

    public User: AccountModel = { Id: "", Email: "", Name: "", Bitcoin: "", Profile: 0, ProfileName: "", Lang: 2, Medals: [], Payments: [], Videos: [], Value: 0, Xp: 0, NextXp: 0, Ap: 0, NextAp: 0, Rp: 0, NextRp: 0, Op: 0, NextOp: 0, Qp: 0, NextQp: 0, Tp: 0, NextTp: 0, Up: 0, NextUp: 0, Sp: 0, NextSp: 0, Ep: 0, NextEp: 0, NewsLetter: false, GeneralQualification: "", TotalQualification: "", EditorQualification: "", ReporterQualification: "", RoleTranslatorEn: 0, RoleTranslatorPt: 0, RoleTranslatorEs: 0, RoleRevisorEn: 0, RoleRevisorPt: 0, RoleRevisorEs: 0, RoleNarratorEn: 0, RoleNarratorPt: 0, RoleNarratorEs: 0, RoleProducerEn: 0, RoleProducerPt: 0, RoleProducerEs: 0, Translator: 0, Revisor: 0, Narrator: 0, Producer: 0, Sponsor: 0, Staff: 0, Admin: 0, Blocked: 0, NotConfirmed: 0 };

    public Targets: TargetModel[] = [];
    public Articles: ArticleModel[] = [];
    public Videos: VideoModel[] = [];

}

export class RegisterGrant {

    public ActionId: string = "";
    public AwardId: string = "";
    public Add: number = 0;
    public Lang: number = 0;

}

export class NewsLetterRegister {

    public Type: number = 0;
    public Data: string = "";
    public Lang?: number = 0;

}

export class ChangeLanguage {

    public Id?: string = "";
    public Lang?: number = 0;
    public NewLang?: number = 0;

}
