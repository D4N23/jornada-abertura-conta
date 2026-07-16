import { JourneyResolutionRequest, JourneyResolutionResponse } from "@/contracts/journey";

const journeyBffUrl = process.env.NEXT_PUBLIC_JOURNEY_BFF_URL

export async function resolveJourneyEntryPoint(cpf: string) : Promise<JourneyResolutionResponse>{
    if (!journeyBffUrl) {
        throw new Error(
            "A variável NEXT_PUBLIC_JOURNEY_BFF_URL não foi configurada."
        );        
    }

    const request: JourneyResolutionRequest = {
        identifier: {
         type: "CPF",
         value: cpf
        }
    };

    const response = await fetch(
        `${journeyBffUrl}/v1/journeys/entry-point/resolve`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(request)
        }
    );

    if (!response.ok) {
        const responseBody = await response.text();

        console.error("Journey BFF response:", {
            status: response.status,
            body: responseBody
        });

        throw new Error( `Não foi possivel consultar a jornada. Status: ${response.status}`)
    }

    return response.json() as Promise<JourneyResolutionResponse>
}