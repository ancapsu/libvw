import { AccountModel } from './Account';
import { NewsCategory, UserIdModel, AuthorInfo, CategInfo, NewsAward } from './News';
import { TargetModel } from './Target';

//
//  The classes here should match webapi models
//

export class ArticleActionGantModel {

    public AwardId: string = "";
    public GrantedBy: string = "";
    public Granted: string = "";

}

export class ArticleActionAudioModel {

    public FileName: string = "";
    public MimeType: string = "";
    public Extension: string = "";

}

export class ArticleActionObservationModel {

    public Observation: string = "";
    public IncludedBy: string = "";
    public Included: string = "";
    public IncludedById: string = "";

}

export class ArticleActionModel {

    public Id: string = "";
    public UserId: string = "";
    public UserName: string = "";
    public Type: number = 0;
    public TypeName: string = "";
    public Observation: string = "";
    public Date: string = "";
    public BillableWords: number = 0;

    public Audios: ArticleActionAudioModel[] = [];
    public Grants: ArticleActionGantModel[] = [];
    public Observations: ArticleActionObservationModel[] = [];

}

export class ArticleLinkModel {

    public Type: number = 0;
    public TypeName: string = "";
    public Link: string = "";
    public Description: string = "";

}

export class ArticleCommentModel {

    public Id: string = "";
    public Comment: string = "";
    public IncludedBy: string = "";
    public Included: string = "";
    public IncludedById: string = "";
  
}

export class ArticleModel {

    public Id: string = "";
    public Title: string = "";
    
    public Text: string = "";
    public Paragraphs: string[] = [];
    public StartingText: string = "";

    public Authors: AuthorInfo = { AuthoredLabel: "", SuggestedLabel: "", RevisedLabel: "", NarratedLabel: "", ProducedLabel: "", Authored: { Id: "", Name: "" }, Suggested: { Id: "", Name: "" }, Revised: { Id: "", Name: "" }, Narrated: { Id: "", Name: "" }, Produced: { Id: "", Name: "" }, DateLabel: "", Date: "", StatusText: "" };

    public Target: TargetModel = { Id: "", Title: "", Link: "", StartingText: "", Text: "", Paragraphs: [], Authors: { AuthoredLabel: "", SuggestedLabel: "", RevisedLabel: "", NarratedLabel: "", ProducedLabel: "", Authored: { Id: "", Name: "" }, Suggested: { Id: "", Name: "" }, Revised: { Id: "", Name: "" }, Narrated: { Id: "", Name: "" }, Produced: { Id: "", Name: "" }, DateLabel: "", Date: "", StatusText: "" }, Categories: { MainCategory: { Label: "", Category: "" }, Categories: [] }, VoteWrite: 0, VoteVery: 0, VoteGood: 0, VoteNot: 0, VoteOld: 0, VoteFake: 0, UserVote: -1, Actions: [], Status: 0, StatusName: "", AssociatedArticleId: "", AssociatedArticleTitle: "" };

    public Categories: CategInfo = { MainCategory: { Label: "", Category: "" }, Categories: [] };

    public Actions: ArticleActionModel[] = [];
    public Links: ArticleLinkModel[] = [];
    public Comments: ArticleCommentModel[] = [];

    public Status: number = 0;
    public StatusNarration: number = 0;
    public Type: number = 0
    public ArticleType: number = 0
    public StatusName: string = "";
    public StatusNarrationName: string = "";
    public TypeName: string = "";
    public ArticleTypeName: string = "";

    public VoteApprove: number = 0;
    public VoteNot: number = 0;
    public VoteAlready: number = 0;
    public VotePoorly: number = 0;

    public UserVote: number = -1;

    public preferredRevisor: string = "";
    public preferredNarrator: string = "";
    public preferredProducer: string = "";

    public preferredRevisorName: string = "";
    public preferredNarratorName: string = "";
    public preferredProducerName: string = "";

    public beingRevised: string = "";
    public beingNarrated: string = "";
    public beingProduced: string = "";

    public beingRevisedName: string = "";
    public beingNarratedName: string = "";
    public beingProducedName: string = "";

}

// -------
//
//   Modelos genéricos do article
//
// -------

export class ArticleRowModel {

    public Articles: ArticleModel[] = [];

}

export class ArticleListModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Sts: number = 0;
    public Ini: number = 0;
    public Total: number = 0;
    public Articles: ArticleModel[] = [];

}

export class ArticleCategoryModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Title: string = "";
    public Description: string = "";

    public Ini: number = 0;
    public Total: number = 0;
    public Articles: ArticleModel[] = [];

}

export class EditArticleBaseModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Targets: TargetModel[] = [];
    public Categories: NewsCategory[] = [];
    public Awards: NewsAward[] = [];
    public Revisors: UserIdModel[] = [];
    public Narrators: UserIdModel[] = [];
    public Producers: UserIdModel[] = [];

}

// -------
//
//   Alterar infos do artigo
//
// -------

export class NewArticleModel {

    public TargetId?: string = "";
    public Title?: string = "";
    public Text?: string = "";
    public Image?: string = "";
    public Type?: number = 0;
    public Categ?: string = "";
    public Lang?: number = 0;

}

export class ChangeArticleModel {

    public Id?: string = "";
    public TargetId?: string = "";
    public Title?: string = "";
    public Text?: string = "";
    public Image?: string = "";
    public Type?: number = 0;
    public Categ?: string = "";
    public Status?: number = 0;
    public Action?: number = 0;
    public Lang?: number = 0;

}

export class ActionFile {

    public FileName?: string = "";
    public Content?: string = "";

}

export class Publish {

    public Id: string = "";
    public Status: number = 0;
    public Info: string = "";
    public Lang?: number = 0;

}

export class IncludeActionWithFile {

    public Id: string = "";
    public Status: number = 0;
    public Info: string = "";
    public Files: ActionFile[] = [];
    public Lang?: number = 0;

}

export class IncludeObservaton {

    public Id?: string = "";
    public CommentId?: string = "";
    public Comment?: string = "";
    public UserName?: string = "";
    public UserId?: string = "";
    public Included?: string = "";
    public Lang?: number = 0;

}

export class RegisterPriority {

    public Priority: number = 0;
    public Define: number = 0;
    public Id: string = "";
    public Lang?: number = 0;
    
}

export class RegisterPriorityPayload {

    public Priority: number = 0;
    public Define: number = 0;
    public Id: string = ""; 
    public UserId: string = "";
    public UserName: string = "";
    public Lang?: number = 0;

}

export class SearchArticleModel {

    public SearchString?: string = "";
    public Lang?: number = 0;

}

export class SearchArticleActionModel {

    public SearchData?: SearchArticleModel;
    public List?: ArticleListModel;

}

export class UserValue
{

    public Id: string = "";
    public Name: string = "";
    public Value: number = 0;

}

export class ValuesPerVideo
{

    public Id: string = "";
    public Title: string = "";
    public Date: string = "";
    public Month: string = "";

    public Author: UserValue = { Id: "", Name: "", Value: 0 };
    public Revisor: UserValue = { Id: "", Name: "", Value: 0 };
    public Narrator: UserValue = { Id: "", Name: "", Value: 0 };
    public Producer: UserValue = { Id: "", Name: "", Value: 0 };

    public Total: number = 0;
    
}

export class VideoValue
{

    public Id: string = "";
    public Title: string = "";
    public Date: string = "";
    public Role: string = "";
    public Value: number = 0;

}

export class ValuesPerUser
{

    public Id: string = "";
    public Name: string = "";
    public Bicoin: string = "";
    public Month: string = "";
    public Description: string = "";

    public Values: VideoValue[] = [];

    public Total: number = 0;

}

export class MonthValueDiscrimination {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Month: string = "";

    public Users: ValuesPerUser[] = [];
    public Videos: ValuesPerVideo[] = [];

    public Total: number = 0;

}
