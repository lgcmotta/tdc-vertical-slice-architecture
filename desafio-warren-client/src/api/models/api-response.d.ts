interface Response<TObject> {
    payload: TObject;
    failures: IFailure[]
}

interface IFailure {
    propertyName: string;
    errorMessage: string;
}