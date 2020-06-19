import { AccountModel } from './Account';
import { AuthorInfo, CategInfo, NewsCategory, NewsAward } from './News';

// -------
//
//   Pegar infos da pauta
//
// -------

export class TargetActionGantModel {

    public AwardId: string = "";
    public GrantedBy: string = "";
    public Granted: string = "";

}

export class TargetActionModel {

    public Id: string = "";
    public UserId: string = "";
    public UserName: string = "";
    public Type: number = 0;
    public TypeName: string = "";
    public Observation: string = "";
    public Date: string = "";

    public Grants: TargetActionGantModel[] = [];

}

export class TargetModel {

    public Id: string = "";
    public Title: string = "";
    public Link: string = "";

    public StartingText: string = "";
    public Text: string = "";
    public Paragraphs: string[] = [];

    public Authors: AuthorInfo = { AuthoredLabel: "", SuggestedLabel: "", RevisedLabel: "", NarratedLabel: "", ProducedLabel: "", Authored: { Id: "", Name: "" }, Suggested: { Id: "", Name: "" }, Revised: { Id: "", Name: "" }, Narrated: { Id: "", Name: "" }, Produced: { Id: "", Name: "" }, DateLabel: "", Date: "", StatusText: "" };
    public Categories: CategInfo = { MainCategory: { Label: "", Category: "" }, Categories: [] };

    public VoteWrite: number = 0;
    public VoteVery: number = 0;
    public VoteGood: number = 0;
    public VoteNot: number = 0;
    public VoteOld: number = 0;
    public VoteFake: number = 0;

    public UserVote: number = -1;

    public Actions: TargetActionModel[] = [];

    public Status: number = 0;
    public StatusName: string = "";

    public AssociatedArticleId: string = "";
    public AssociatedArticleTitle: string = "";

}

// -------
//
//   Modelos genéricos da pauta
//
// -------

export class TargetListModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Ini: number = 0;
    public Total: number = 0;
    public Targets: TargetModel[] = [];

}

export class TargetCategoryModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Title: string = "";
    public Description: string = "";

    public Ini: number = 0;
    public Total: number = 0;
    public Targets: TargetModel[] = [];

}

export class EditTargetBaseModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Categories: NewsCategory[] = [];
    public Awards: NewsAward[] = [];

}

// -------
//
//   Alterar infos da pauta
//
// -------

export class LinkModel {

    public Link?: string;
    public Lang?: number = 0;

}

export class LinkResultModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Link: string = "";
    public Title: string = "";
    public Text: string = "";
    public Image: string[] = [];

}

export class NewTargetModel {

    public Title?: string = "";
    public Link?: string = "";
    public Text?: string = "";
    public ImageType?: number = 0;
    public Image?: string = "";
    public Categ?: string = "";
    public Lang?: number = 0;

}

export class ChangeTargetModel {

    public Id?: string = "";
    public Title?: string = "";
    public Link?: string = "";
    public Text?: string = "";
    public Image?: string = "";
    public Categ?: string = "";
    public Status?: number = 0;
    public Action?: number = 0;
    public Lang?: number = 0;

}

export class RegisterVote {

    public Id?: string = "";
    public Vote?: number = 0;
    public Lang?: number = 0;

}

export class TargetActionGrantModel {

    public TargetActionId?: string = "";
    public GrantType?: number = 0;
    public Lang?: number = 0;

}

export class SearchTargetModel {

    public SearchString?: string = "";
    public Lang?: number = 0;

}

export class SearchTargetActionModel {

    public SearchData?: SearchTargetModel;
    public List?: TargetListModel;

}