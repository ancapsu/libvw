import { AccountModel } from './Account';
import { AuthorInfo, CategInfo, NewsCategory } from './News';

export class VideoActionGantModel {

    public AwardId: string = "";
    public GrantedBy: string = "";
    public Granted: string = "";

}

export class VideoActionModel {

    public Id: string = "";
    public UserId: string = "";
    public UserName: string = "";
    public Type: number = 0;
    public TypeName: string = "";
    public Observation: string = "";
    public Date: string = "";

    public Grants: VideoActionGantModel[] = [];

}

export class VideoModel {

    public Id: string = "";
    public Title: string = "";
    public YoutubeLink: string = "";
    public BitchuteLink: string = "";

    public StartingDescription: string = "";
    public Description: string = "";
    public DescriptionPars: string[] = [];
    public Script: string = "";
    public ScriptPars: string[] = [];
    public Tags: string = "";

    public Authors: AuthorInfo = { AuthoredLabel: "", SuggestedLabel: "", RevisedLabel: "", NarratedLabel: "", ProducedLabel: "", Authored: { Id: "", Name: "" }, Suggested: { Id: "", Name: "" }, Revised: { Id: "", Name: "" }, Narrated: { Id: "", Name: "" }, Produced: { Id: "", Name: "" }, DateLabel: "", Date: "", StatusText: "" };
    public Categories: CategInfo = { MainCategory: { Label: "", Category: "" }, Categories: [] };

    public Actions: VideoActionModel[] = [];

    public Status: number = 0;
    public StatusName: string = "";

}

// -------
//
//   Modelos genéricos de video
//
// -------

export class VideoListModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Ini: number = 0;
    public Total: number = 0;
    public Videos: VideoModel[] = [];

}

export class VideoCategoryModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Title: string = "";
    public Description: string = "";

    public Ini: number = 0;
    public Total: number = 0;
    public Videos: VideoModel[] = [];

}

export class YoutubeModel {

    public Link?: string;
    public Lang?: number;

}

export class YoutubeResultModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Title: string = "";
    public Description: string = "";
    public Tags: string = "";
    public YoutubeLink: string = "";
    public BitchuteLink: string = "";
    public Image: string = "";

}

export class EditVideoBaseModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Categories: NewsCategory[] = [];

}

// -------
//
//   Alteração do vídeo
//
// -------

export class NewVideoModel {

    public Title?: string = "";
    public Description?: string = "";
    public Tags?: string = "";
    public YoutubeLink?: string = "";
    public BitchuteLink?: string = "";
    public Image?: string = "";
    public Categ?: string = "";
    public Script?: string = "";
    public Lang?: number = 0;

}

export class ChangeVideoModel {

    public Id?: string = "";
    public Title?: string = "";
    public Description?: string = "";
    public Tags?: string = "";
    public YoutubeLink?: string = "";
    public BitchuteLink?: string = "";
    public Image?: string = "";
    public Categ?: string = "";
    public Status?: number = 0;
    public Action?: number = 0;
    public Script?: string = "";
    public Lang?: number = 0;

}

export class SearchVideoModel {

    public SearchString?: string = "";
    public Lang?: number = 0;

}

export class SearchVideoActionModel {

    public SearchData?: SearchVideoModel;
    public List?: VideoListModel;

}