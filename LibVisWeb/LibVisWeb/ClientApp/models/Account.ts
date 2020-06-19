import { ValuesPerVideo } from "./Article";

//
//  The classes here should match webapi models
//

export class AccountModel {

    public Id: string = "";
    public Email: string = "";
    public Name: string = "";

    public Bitcoin: string = "";

    public Profile: number = 0;
    public ProfileName: string = "";

    public Medals: MedalsModel[] = [];
    public Payments: PaymentModel[] = [];
    public Videos: ValuesPerVideoPerUser[] = [];
    public Value: number = 0;

    public Xp: number = 0;
    public NextXp: number = 0;
    public Ap: number = 0;
    public NextAp: number = 0;
    public Rp: number = 0;
    public NextRp: number = 0;
    public Op: number = 0;
    public NextOp: number = 0;
    public Qp: number = 0;
    public NextQp: number = 0;
    public Tp: number = 0;
    public NextTp: number = 0;
    public Up: number = 0;
    public NextUp: number = 0;
    public Sp: number = 0;
    public NextSp: number = 0;
    public Ep: number = 0;
    public NextEp: number = 0;

    public NewsLetter: boolean = false;

    public GeneralQualification: string = "";
    public TotalQualification: string = "";
    public EditorQualification: string = "";
    public ReporterQualification: string = "";

    public Lang: number = 0;

    public RoleTranslatorEn: number = 0;
    public RoleTranslatorPt: number = 0;
    public RoleTranslatorEs: number = 0;

    public RoleRevisorEn: number = 0;
    public RoleRevisorPt: number = 0;
    public RoleRevisorEs: number = 0;

    public RoleNarratorEn: number = 0;
    public RoleNarratorPt: number = 0;
    public RoleNarratorEs: number = 0;

    public RoleProducerEn: number = 0;
    public RoleProducerPt: number = 0;
    public RoleProducerEs: number = 0;

    public Translator: number = 0;
    public Revisor: number = 0;
    public Narrator: number = 0;
    public Producer: number = 0;
    public Sponsor: number = 0;
    public Staff: number = 0;
    public Admin: number = 0;

    public Blocked: number = 0;
    public NotConfirmed: number = 0;

}

export class CreateAccountModel {

    public Name?: string;
    public Email?: string;
    public Password?: string;
    public ConfirmPassword?: string;
    public Lang?: number;

}

export class SendAgainModel {

    public Email?: string;
    public Type?: number;
    public Lang?: number;

}

export class AccountChangeModel {

    public Name?: string;
    public Email?: string;
    public Bitcoin?: string;
    public Avatar?: string;
    public NewsLetter?: boolean;
    public Lang?: number;
    
}

export class ChangeLanguageModel {

    public Lang?: number;

}

export class ForgotPasswordModel {

    public Email?: string;
    public Lang?: number;

}

export class ConfirmEmailModel {

    public Email?: string;
    public Code?: string;
    public Lang?: number;

}

export class ChangePasswordModel {
    
    public OldPassword?: string;
    public NewPassword?: string;
    public ConfirmNewPassword?: string;
    public Lang?: number;

}

export class RecoverPasswordModel {

    public Email?: string;
    public ChangeToken?: string;
    public NewPassword?: string;
    public ConfirmNewPassword?: string;
    public Lang?: number;

}

export class MedalsModel {

    public Id: string = "";
    public Name: string = "";
    public Description: string = "";

}

export class PaymentModel {

    public Id: string = "";
    public MonthYearRef: string = "";
    public NumArticles: number = 0;
    public Value: string = "";
    public Observation: string = "";
    public Address: string = "";
    public TransactionId: string = "";
    public Payed: string = "";

}

export class FileDataModel {

    public Token: string = "";
    public Data: string = "";
    
}

export class LoginRequestModel {

    public Login?: string;
    public Password?: string;
    public KeepLogged?: boolean;
    public Lang?: number;

}

export class LoginResultModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public LastLoginDate: string = "";
    public NumberOfTentatives: number = 0;

    public Token: string = "";

    public Account: AccountModel = { Id: "", Email: "", Name: "", Bitcoin: "", Profile: 0, ProfileName: "", Lang: 2, Medals: [], Payments: [], Videos: [], Value: 0, Xp: 0, NextXp: 0, Ap: 0, NextAp: 0, Rp: 0, NextRp: 0, Op: 0, NextOp: 0, Qp: 0, NextQp: 0, Tp: 0, NextTp: 0, Up: 0, NextUp: 0, Sp: 0, NextSp: 0, Ep: 0, NextEp: 0, NewsLetter: false, GeneralQualification: "", TotalQualification: "", EditorQualification: "", ReporterQualification: "", RoleTranslatorEn: 0, RoleTranslatorPt: 0, RoleTranslatorEs: 0, RoleRevisorEn: 0, RoleRevisorPt: 0, RoleRevisorEs: 0, RoleNarratorEn: 0, RoleNarratorPt: 0, RoleNarratorEs: 0, RoleProducerEn: 0, RoleProducerPt: 0, RoleProducerEs: 0, Translator: 0, Revisor: 0, Narrator: 0, Producer: 0, Sponsor: 0, Staff: 0, Admin: 0, Blocked: 0, NotConfirmed: 0 };

}

export class ValuesPerVideoPerUser {

    public Id: string = "";
    public Title: string = "";
    public Date: string = "";
    public Role: string = "";

    public Word: number = 0;
    public Total: number = 0;

}

export class SearchUserModel {

    public SearchString?: string = "";
    public Lang?: number = 0;

}

export class UserListModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Ini: number = 0;
    public Total: number = 0;
    public List: AccountModel[] = [];

}

export class SearchUserActionModel {

    public SearchData?: SearchUserModel;
    public List?: UserListModel;

}

export class ManagerChangePasswordModel {

    public Id?: string;
    public NewPassword?: string;
    public ConfirmNewPassword?: string;
    public Lang?: number;

}

export class AccountForManagerModel {

    public Result: number = 0;
    public ResultComplement: string = "";

    public Account: AccountModel | null = null;

}

